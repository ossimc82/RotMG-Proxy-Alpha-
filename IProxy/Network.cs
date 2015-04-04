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
using IProxy.common.data;
using IProxy.Networking;
using IProxy.Networking.ServerPackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IProxy
{
    public class Network
    {
        public delegate void PacketDelegate(Packet packet);

        public event PacketDelegate OnSendToServer;
        public event PacketDelegate OnSendToClient;

        public NetworkClient Client { get; private set; }

        public Network()
        {
            Client = new NetworkClient(this);
        }

        public void SendToServer(Packet packet)
        {
            if (OnSendToServer != null)
                OnSendToServer(packet);
        }

        public void SendToClient(Packet packet)
        {
            if (OnSendToClient != null)
                OnSendToClient(packet);
        }

        public void After(int milliseconds, Action action)
        {
            new Timer(new TimerCallback((obj) =>
            {
                Task.Factory.StartNew(obj as Action);
            }), action, milliseconds, Timeout.Infinite);
        }

        public struct NetworkClient
        {
            private Network network;

            public NetworkClient(Network network)
            {
                this.network = network;
            }

            public void SendInfo(string text)
            {
                SendTell("", text);
            }

            public void SendHelp(string text)
            {
                SendTell("*Help*", text);
            }

            public void SendError(string text)
            {
                SendTell("*Error*", text);
            }

            public void SendGuild(string text)
            {
                SendTell("*Guild*", text);
            }

            public void SendEnemy(string enemy, string text)
            {
                SendTell("#" + enemy, text);
            }

            public void SendAnnouncement(string text)
            {
                SendTell("@ANNOUNCEMENT", text);
            }

            public void SendTell(string from, string text, string to = "", int objId = -1, int stars = -1)
            {
                network.SendToClient(new TextPacket
                {
                    BubbleTime = 10,
                    Name = from,
                    ObjectId = objId,
                    Recipient = to,
                    Stars = stars,
                    Text = text,
                    CleanText = ""
                });
            }
        }
    }
}
