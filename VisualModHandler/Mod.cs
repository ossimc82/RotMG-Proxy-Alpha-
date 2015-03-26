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
using IProxy.Mod.WinForm;
using IProxy.Networking;
using IProxy.Networking.ClientPackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualModHandler
{
    public class Mod : IProxyMod
    {
        public string Name
        {
            get { return "Visual Mod Handler"; }
        }

        public string Description
        {
            get { return "This mod will add a visual handling for your mods."; }
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
            get { return "Ingame Commands:\n\r\"/visual\" to open this window."; }
        }

        public void Create()
        {
            Singleton<Mod>.SetInstance(this);
        }
    }

    public class PacketHandler : PacketHandlerExtentionBase
    {
        public override bool OnClientPacketReceived(ref Packet packet)
        {
            switch (packet.ID)
            {
                case PacketID.PLAYERTEXT:
                    if (Utils.ChangePacketType<PlayerTextPacket>(packet).Text == "/visual")
                    {
                        Singleton<FormHost>.Instance.SetNextForm(typeof(MainForm));
                        return false;
                    }
                    break;
            }
            return base.OnClientPacketReceived(ref packet);
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

    public class FormHost : WinFormProviderExtentionBase, IWinFormHost
    {
        public Type GetDefaultForm()
        {
            return typeof(MainForm);
        }

        public bool RunOnLoad()
        {
            return true;
        }

        public override IEnumerable<Type> GetForms()
        {
            yield return typeof(MainForm);
        }

        public override void CreateSingletonForProvider()
        {
            Singleton<FormHost>.SetInstance(this);
        }
    }
}
