//The MIT License (MIT)
//
//Copyright (c) 2015 Fabian Fischer
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
using IProxy;
using IProxy.DataSerializing;
using IProxy.Networking;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proxy
{
    public sealed class NetworkCore
    {
        private static ILog log = LogManager.GetLogger(typeof(NetworkCore));

        public const int PACKET_HEADER_SIZE = 5;

        private Socket clientSocket;
        private Thread wkr;
        private Client parent;
        private TcpClient dest;

        private object clientSend;
        private object serverSend;

        internal RC4 ClientReceiveKey { get; private set; }
        internal RC4 ClientSendKey { get; private set; }
        internal RC4 ServerReceiveKey { get; private set; }
        internal RC4 ServerSendKey { get; private set; }

        public delegate bool OnPacketReceiveEventHandler(ref Packet packet);

        public event OnPacketReceiveEventHandler OnClientPacketReceive;
        public event OnPacketReceiveEventHandler OnServerPacketReceive;

        struct RawPacket
        {
            public byte id;
            public byte[] content;
        }

        public NetworkCore(Socket clientSocket, Client client)
        {
            this.clientSocket = clientSocket;
            this.parent = client;
            ClientReceiveKey = new RC4(new byte[] { 0x31, 0x1f, 0x80, 0x69, 0x14, 0x51, 0xc7, 0x1d, 0x09, 0xa1, 0x3a, 0x2a, 0x6e });
            ClientSendKey = new RC4(new byte[] { 0x72, 0xc5, 0x58, 0x3c, 0xaf, 0xb6, 0x81, 0x89, 0x95, 0xcd, 0xd7, 0x4b, 0x80 });
            ServerReceiveKey = new RC4(new byte[] { 0x72, 0xc5, 0x58, 0x3c, 0xaf, 0xb6, 0x81, 0x89, 0x95, 0xcd, 0xd7, 0x4b, 0x80 });
            ServerSendKey = new RC4(new byte[] { 0x31, 0x1f, 0x80, 0x69, 0x14, 0x51, 0xc7, 0x1d, 0x09, 0xa1, 0x3a, 0x2a, 0x6e });

            clientSend = new object();
            serverSend = new object();
        }

        public void BeginProcess()
        {
            dest = new TcpClient();
            dest.Connect(Singleton<Server>.Instance.CurrentHost, 2050);

            wkr = new Thread(StartNetworkTasks);
            wkr.Start();
        }

        private void StartNetworkTasks()
        {
            Task.Factory.StartNew(ReadFromClient);
            Task.Factory.StartNew(ReadFromServer);
        }

        private void ReadFromClient()
        {
            try
            {
                var rdr = new DReader(new NetworkStream(clientSocket));
                while (true)
                {
                    int len;
                    byte id;
                    byte[] content;
                    try
                    {
                        len = rdr.ReadInt32();
                        id = rdr.ReadByte();
                        content = rdr.ReadBytes(len - PACKET_HEADER_SIZE);
                    }
                    catch (IOException)
                    {
                        break;
                    }

                    content = ClientReceiveKey.Crypt(content);

                    if (OnClientPacketReceive != null)
                    {
                        Packet pkt = Packet.Packets[(PacketID)id].CreateInstance();
                        pkt.Read(content, len - PACKET_HEADER_SIZE);

                        if (this.OnClientPacketReceive(ref pkt))
                            SendToServer(pkt);
                        else
                            log.InfoFormat("Skip sending packet, abort by user:\n{0}", pkt.ToString());
                    }
                    else
                        m_sendToServer(new RawPacket { id = id, content = content });
                }
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                Disconnect();
            }
        }

        private void ReadFromServer()
        {
            try
            {
                var rdr = new DReader(dest.GetStream());

                while (true)
                {
                    int len;
                    byte id;
                    byte[] content;
                    try
                    {
                        len = rdr.ReadInt32();
                        id = rdr.ReadByte();
                        content = rdr.ReadBytes(len - PACKET_HEADER_SIZE);
                    }
                    catch (IOException)
                    {
                        break;
                    }

                    content = ServerReceiveKey.Crypt(content);

                    if (OnServerPacketReceive != null)
                    {
                        Packet pkt = Packet.Packets[(PacketID)id].CreateInstance();
                        pkt.Read(content, len - PACKET_HEADER_SIZE);

                        if (this.OnServerPacketReceive(ref pkt))
                            SendToClient(pkt);
                        else
                            log.InfoFormat("Skip sending packet, abort by user:\n{0}", pkt.ToString());

                    }
                    else
                        m_sendToClient(new RawPacket { id = id, content = content });
                }
            }
            catch(ObjectDisposedException) { }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                Disconnect();
            }
        }

        public void Disconnect()
        {
            if (clientSocket != null && clientSocket.IsBound)
                clientSocket.Close();
            if (dest != null && dest.Client.IsBound)
                dest.Close();

            if (wkr.ThreadState != ThreadState.Running)
                wkr.Abort();

            Singleton<ModHandler>.Instance.Disconnect();
        }

        public void SendToClient(Packet pkt)
        {
            byte[] buff = new byte[0];
            pkt.Write(ref buff);
            m_sendToClient(new RawPacket { id = (byte)pkt.ID, content = buff });
        }

        public void SendToServer(Packet pkt)
        {
            byte[] packet = new byte[0];
            pkt.Write(ref packet);
            m_sendToServer(new RawPacket { id = (byte)pkt.ID, content = packet });
        }

        private void m_sendToClient(RawPacket packet)
        {
            if (!clientSocket.Connected) return;
            lock (clientSend)
            {
                try
                {
                    var wtr = new DWriter(new NetworkStream(clientSocket));
                    wtr.Write(packet.content.Length + PACKET_HEADER_SIZE);
                    wtr.Write(packet.id);
                    wtr.Write(ClientSendKey.Crypt(packet.content));
                    wtr.Flush();
                }
                //Only occures when the socket is closed.
                catch (ObjectDisposedException) { }
                catch (IOException) { }
            }
        }

        private void m_sendToServer(RawPacket packet)
        {
            if (!dest.Connected) return;
            lock (serverSend)
            {
                try
                {
                    var wtr = new DWriter(new NetworkStream(dest.Client));
                    wtr.Write(packet.content.Length + PACKET_HEADER_SIZE);
                    wtr.Write(packet.id);
                    wtr.Write(ServerSendKey.Crypt(packet.content));
                    wtr.Flush();
                }
                //Only occures when the socket is closed.
                catch (ObjectDisposedException) { }
                catch (IOException) { }
            }
        }
    }
}
