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
using IProxy.Networking.ServerPackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlowEnabler
{
    public class Mod : IProxyMod
    {
        public string Name
        {
            get { return "Glow Enabler"; }
        }

        public string Description
        {
            get { return "This makes you glow"; }
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
            GetValue<bool>("glowing", "true");
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

        public IEnumerable<KeyValuePair<string, string>> GetValues()
        {
            return settings.GetValues();
        }

        public void Dispose()
        {
            settings.Dispose();
        }
    }

    public class PacketHandler : AdvancedPacketHandlerExtentionBase
    {
        private int myObjId;

        protected override void HookPackets()
        {
            ApplyPacketHook<CreateSuccessPacket>(OnCreateSuccess);
            ApplyPacketHook<UpdatePacket>(OnUpdate);
            ApplyPacketHook<NewTickPacket>(OnNewTick);
        }

        private bool OnUpdate(ref UpdatePacket packet)
        {
            for (int i = 0; i < packet.NewObjects.Length; i++)
            {
                if (packet.NewObjects[i].Stats.Id == myObjId)
                {
                    for (int j = 0; j < packet.NewObjects[i].Stats.Stats.Length; j++)
                    {
                        if (packet.NewObjects[i].Stats.Stats[j].Key == StatsType.Glowing)
                            packet.NewObjects[i].Stats.Stats[j] = new KeyValuePair<StatsType, object>(StatsType.Glowing, Singleton<Settings>.Instance.GetValue<bool>("glowing").ToInt32() - 1);
                    }
                }
            }
            return true;
        }

        private bool OnNewTick(ref NewTickPacket packet)
        {
            for (int i = 0; i < packet.UpdateStatuses.Length; i++)
            {
                if (packet.UpdateStatuses[i].Id == myObjId)
                {
                    for (int j = 0; j < packet.UpdateStatuses[i].Stats.Length; j++)
                    {
                        if (packet.UpdateStatuses[i].Stats[j].Key == StatsType.Glowing)
                            packet.UpdateStatuses[i].Stats[j] = new KeyValuePair<StatsType, object>(StatsType.Glowing, Singleton<Settings>.Instance.GetValue<bool>("glowing").ToInt32() - 1);
                    }
                }
            }
            return true;
        }

        private bool OnCreateSuccess(ref CreateSuccessPacket packet)
        {
            myObjId = packet.ObjectID;
            return true;
        }
    }
}
