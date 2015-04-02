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
using IProxy.Mod;
using IProxy.Networking;
using IProxy.Networking.ServerPackets;
using RealmManager.Entities;
using RealmManager.realm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealmManager
{
    public class Mod : IProxyMod
    {
        public string Name
        {
            get { return "Realm Manager"; }
        }

        public string Description
        {
            get { return "This will provide some basic classes to the proxy and with some stored information about the current session, like current map name and the current player data."; }
        }

        public string Creator
        {
            get { return "ossimc82"; }
        }

        public string RequiredMinimumProxyVersion
        {
            get { return "1.1.0"; }
        }

        public string ModVersion
        {
            get { return "Indev 0.0.1"; }
        }

        public string Help
        {
            get { return null; }
        }

        public void Create()
        {
            Singleton<World>.SetInstance(new World());
            Singleton<Mod>.SetInstance(this);
        }
    }

    public class PacketHandler : PacketHandlerExtentionBase
    {
        private int myObjectId;

        public override bool OnServerPacketReceived(ref Packet packet)
        {
            switch (packet.ID)
            {
                case PacketID.MAPINFO:
                    Singleton<World>.Instance.UpdateWorld(Utils.ChangePacketType<MapInfoPacket>(packet));
                    break;
                case PacketID.CREATE_SUCCESS:
                    myObjectId = Utils.ChangePacketType<Create_SuccessPacket>(packet).ObjectID;
                    break;
                case PacketID.UPDATE:
                    parseUpdate(Utils.ChangePacketType<UpdatePacket>(packet));
                    break;
                case PacketID.NEW_TICK:
                    parseNewTick(Utils.ChangePacketType<NewTickPacket>(packet));
                    break;
            }
            return base.OnClientPacketReceived(ref packet);
        }

        private void parseNewTick(NewTickPacket newTickPacket)
        {
            foreach (var i in newTickPacket.UpdateStatuses)
            {
                Entity en = Singleton<World>.Instance.GetEntity(i.Id);
                if (en != null)
                    en.ImportStats(i);

                if (i.Id == myObjectId)
                    Singleton<Player>.Instance.ImportStats(i);
            }
        }

        private void parseUpdate(UpdatePacket updatePacket)
        {
            foreach (var i in updatePacket.NewObjects)
            {
                Entity en = Singleton<World>.Instance.GetEntity(i.Stats.Id, i.ObjectType);
                en.ImportStats(i.Stats);

                if (i.Stats.Id == myObjectId)
                {
                    if (!Singleton<Player>.IsSealed)
                        Singleton<Player>.SetInstance(en as Player);
                    Singleton<Player>.Instance.ImportStats(i.Stats);
                }
            }
        }
    }
}
