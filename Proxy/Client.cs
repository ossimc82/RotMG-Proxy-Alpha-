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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Net;
using IProxy.Networking;
using IProxy.Networking.ServerPackets;
using IProxy;
using IProxy.Networking.ClientPackets;
using log4net;

namespace Proxy
{
    public sealed class Client : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Client));

        private NetworkCore core;

        public Client(Socket skt)
        {
            core = new NetworkCore(skt, this);
            core.BeginProcess();

            core.OnClientPacketReceive += core_OnClientPacketReceive;
            core.OnServerPacketReceive += core_OnServerPacketReceive;
            log.Info("Received client.");
        }

        private bool core_OnServerPacketReceive(ref Packet packet)
        {
            switch (packet.ID)
            {
                case PacketID.RECONNECT:
                    Singleton<Server>.Instance.CurrentHost = String.IsNullOrWhiteSpace((packet as ReconnectPacket).Host) ? Singleton<Server>.Instance.DefaultHost : (packet as ReconnectPacket).Host;
                    Singleton<Server>.Instance.CurrentPort = (packet as ReconnectPacket).Port == -1 ? Singleton<Server>.Instance.DefaultPort : (packet as ReconnectPacket).Port;

                    (packet as ReconnectPacket).Host = "localhost";
                    (packet as ReconnectPacket).Port = 2050;
                    return true;

                case PacketID.CREATE_SUCCESS:
                    var pkt = packet;
                    Singleton<Network>.Instance.After(1200, new Action(() => SendToClient(new NotificationPacket
                    {
                        Color = new ARGB(0x00ff00),
                        ObjectId = Utils.ChangePacketType<CreateSuccessPacket>(pkt).ObjectID,
                        Text = Utils.TextToLanguageString("Proxy Enabled.")
                    })));
                    break;
            }
            return Singleton<ModHandler>.Instance.OnServerPacketReceive(ref packet);
        }
        
        private bool core_OnClientPacketReceive(ref Packet packet)
        {
            return Singleton<ModHandler>.Instance.OnClientPacketReceive(ref packet);
        }

        internal void SendToClient(Packet packet)
        {
            core.SendToClient(packet);
        }

        internal void SendToServer(Packet packet)
        {
            core.SendToServer(packet);
        }

        public void Disconnect()
        {
            core.Disconnect();
        }

        public void Dispose()
        {
            core.Disconnect();
        }
    }
}
