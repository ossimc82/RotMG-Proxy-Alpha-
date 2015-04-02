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
using RealmManager.entities;
using RealmManager.Entities;
using RealmManager.realm;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            get { return "Auto nexus with less or equal than the specific amount"; }
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
            get { return "1.0.2"; }
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
            GetValue<float>("autoNexusPercent", "35.0");
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

    public class PacketHandler : PacketHandlerExtentionBase, ICommandManager
    {
        public override bool OnClientPacketReceived(ref Packet packet)
        {
            if (Singleton<Player>.Instance.HP != Int32.MaxValue && Singleton<Player>.Instance.StatValues[Stat.MAX_HP] != 0 && packet.ID != PacketID.HELLO && packet.ID != PacketID.LOAD)
                if (!checkForAutoNexus()) return false;

            return base.OnClientPacketReceived(ref packet);
        }

        public override bool OnServerPacketReceived(ref Packet packet)
        {
            if (Singleton<Player>.Instance.HP != Int32.MaxValue && Singleton<Player>.Instance.StatValues[Stat.MAX_HP] != 0 && packet.ID != PacketID.RECONNECT && packet.ID != PacketID.MAPINFO)
                if (!checkForAutoNexus()) return false;

            return base.OnServerPacketReceived(ref packet);
        }

        private bool checkForAutoNexus()
        {
            if (Singleton<World>.Instance.Name == "Nexus") return true;
            float percent = (100F * (float)Singleton<Player>.Instance.HP / (float)Singleton<Player>.Instance.StatValues[Stat.MAX_HP]);
            float nexusPercent = Singleton<Settings>.Instance.GetValue<float>("autoNexusPercent");
            if (percent <= nexusPercent)
            {
                Singleton<Network>.Instance.SendToServer(new EscapePacket());
                return false;
            }
            return true;
        }

        public IEnumerable<string> RegisterCommands()
        {
            yield return "autonexus";
        }

        public bool OnCommandGet(string command, string[] args)
        {
            switch (command)
            {
                case "autonexus":
                    float percent;

                    if (args.Length == 0)
                    {
                        Singleton<Network>.Instance.Client.SendHelp("Usage: /autonexus <HP in Percent (0.0 - 100.0)>");
                        return false;
                    }

                    if (!Single.TryParse(args[0], NumberStyles.Any, CultureInfo.InvariantCulture, out percent))
                    {
                        Singleton<Network>.Instance.Client.SendError(String.Format("{0} is not a float", args[0]));
                        return false;
                    }
                    Console.WriteLine(percent);
                    Singleton<Settings>.Instance.SetValue("autoNexusPercent", percent.ToString("R"));
                    Singleton<Network>.Instance.Client.SendInfo(String.Format("Settings saved. Autonexus at {0}%", Singleton<Settings>.Instance.GetValue<float>("autoNexusPercent").ToString("R")));
                    Singleton<Settings>.Instance.Save();
                    return false;
            }
            return true;
        }
    }
}
