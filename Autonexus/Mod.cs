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
using IProxy.common;
using IProxy.Mod;
using IProxy.Networking;
using IProxy.Networking.ClientPackets;
using IProxy.Networking.ServerPackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Autonexus
{
    public class Mod : IProxyMod
    {
        public string Name
        {
            get { return "Auto Nexus"; }
        }

        public string Description
        {
            get { return "Auto nexus with less or equal than 30% hp"; }
        }

        public string Creator
        {
            get { return "ossimc82"; }
        }

        public string RequiredMinimumProxyVersion
        {
            get { return "1.0.0"; }
        }

        public string ModVersion
        {
            get { return "1.0.0"; }
        }

        public string Help
        {
            get { return null; }
        }

        public void Create()
        {
            Singleton<Mod>.SetInstance(this);
        }
    }

    public class Settings : ISettingsProvider
    {
        private SimpleSettings settings;

        public ISettingsProvider Register(string modId)
        {
            settings = new SimpleSettings(modId);
            return Singleton<Settings>.SetInstance(this);
        }

        public T GetValue<T>(string key, string def = null)
        {
            return settings.GetValue<T>(key, def);
        }

        public void SetValue(string key, string value)
        {
            settings.SetValue(key, value);
        }

        public void Save()
        {
            settings.Save();
        }

        public void Dispose()
        {
            settings.Dispose();
        }

        public IEnumerable<KeyValuePair<string, string>> GetValues()
        {
            return settings.GetValues();
        }
    }

    public class PacketHandler : PacketHandlerExtentionBase
    {
        private int playerObjectId;
        private int maxHP;
        private int curHP;
        private bool stop;

        private Thread hpCheck;

        public PacketHandler()
        {
            this.maxHP = Int32.MaxValue;
            this.curHP = Int32.MaxValue;

            this.hpCheck = new Thread(new ThreadStart(CheckForHp));
            this.hpCheck.Start();
        }

        public override bool DisposeAfterDisconnect()
        {
            return true;
        }

        public override bool OnServerPacketReceived(ref Packet packet)
        {
            switch (packet.ID)
            {
                case PacketID.CREATE_SUCCESS:
                    this.playerObjectId = (packet as Create_SuccessPacket).ObjectID;
                    break;

                case PacketID.UPDATE:
                    foreach(var def in (packet as UpdatePacket).NewObjects)
                    {
                        if (def.Stats.Id == playerObjectId)
                        {
                            foreach (var stat in def.Stats.Stats)
                            {
                                if (stat.Key == StatsType.HP)
                                    curHP = (int)stat.Value;

                                if (stat.Key == StatsType.MaximumHP)
                                    maxHP = (int)stat.Value;
                            }
                        }
                    }
                    break;

                case PacketID.NEW_TICK:
                    foreach (var stat in (packet as NewTickPacket).UpdateStatuses)
                    {
                        if (stat.Id == playerObjectId)
                        {
                            foreach (var data in stat.Stats)
                            {
                                if (data.Key == StatsType.HP)
                                    curHP = (int)data.Value;

                                if (data.Key == StatsType.MaximumHP)
                                    maxHP = (int)data.Value;
                            }
                        }
                    }
                    break;
            }
            return base.OnServerPacketReceived(ref packet);
        }

        public override void OnDisconnect()
        {
            this.stop = true;
        }

        private void CheckForHp()
        {
            while(!this.stop)
            {
                if (curHP == Int32.MaxValue || maxHP == Int32.MaxValue) continue;

                float percent = (100F * (float)curHP / (float)maxHP);
                float nexusPercent = Singleton<Settings>.Instance.GetValue<float>("autoNexusPercent", "30.0");
                if (percent <= nexusPercent)
                {
                    Singleton<Network>.Instance.SendToServer(new EscapePacket());
                    Thread.Sleep(500);
                }
            }
        }
    }
}
