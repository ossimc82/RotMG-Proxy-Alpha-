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
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Proxy
{
    public sealed class ModHandler : IDisposable
    {
        public const string PROXY_MOD_VERSION = "1.0.0";
        public const string MINIMUM_PROXYMOD_VERSION = "0.0.1";

        private readonly static ILog log = LogManager.GetLogger(typeof(ModHandler));

        public static Dictionary<IProxyMod, Mod> Mods = new Dictionary<IProxyMod, Mod>();

        public static Dictionary<string, Assembly> ModDependencyAssemblies = new Dictionary<string, Assembly>();

        public ModHandler()
        {
            Singleton<ModHandler>.SetInstance(this);
            Singleton<Network>.Instance.OnSendToServer += ModHandler_OnSendToServer;
            Singleton<Network>.Instance.OnSendToClient += ModHandler_OnSendToClient;
            Singleton<Network>.Seal();
        }

        private void ModHandler_OnSendToClient(Packet packet)
        {
            Proxy.SendToClient(packet);
        }

        private void ModHandler_OnSendToServer(Packet packet)
        {
            Proxy.SendToServer(packet);
        }

        internal void Disconnect()
        {
            foreach (var mod in Mods.Values)
                mod.Disconnect();
        }

        internal bool OnServerPacketReceive(ref Packet packet)
        {
            bool ret = true;
            foreach (var mod in Mods.Values.Where(_ => _.Enabled && _.PacketHandlerExtentionBase != null))
            {
                try
                {
                    bool result = mod.PacketHandlerExtentionBase.OnServerPacketReceived(ref packet);
                    if (!result && ret) ret = false;
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Error in mod \"{0}\":\n{1}", mod.Information.Name, ex);
                }
            }
            return ret;
        }

        internal bool OnClientPacketReceive(ref Packet packet)
        {
            bool ret = true;
            foreach (var mod in Mods.Values.Where(_ => _.Enabled && _.PacketHandlerExtentionBase != null))
            {
                try
                {
                    bool result = mod.PacketHandlerExtentionBase.OnClientPacketReceived(ref packet);
                    if (!result && ret) ret = false;
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Error in mod \"{0}\":\n{1}", mod.Information.Name, ex);
                }
            }
            return ret;
        }

        public void DisableMod(IProxyMod mod)
        {
            if (Mods.ContainsKey(mod))
                Mods[mod].Disable();
        }

        public void EnableMod(IProxyMod mod)
        {
            if (Mods.ContainsKey(mod))
                Mods[mod].Enable();
        }

        // sometimes I believe compiler ignores all my comments
        internal void AddMod(IProxyMod userMod, Assembly assembly)
        {
            Version modVersion;
            Version proxyVersion;
            Version minProxyModVersion;

            Version.TryParse(userMod.RequiredMinimumProxyVersion, out modVersion);
            Version.TryParse(PROXY_MOD_VERSION, out proxyVersion);
            Version.TryParse(MINIMUM_PROXYMOD_VERSION, out minProxyModVersion);

            log.InfoFormat("Loading Mod: {0} by - {1}", userMod.Name, userMod.Creator);

            if (proxyVersion >= modVersion)
            {
                if (modVersion >= minProxyModVersion)
                {
                    if (proxyVersion > modVersion)
                        log.WarnFormat("[{0}] Modloader version is higher than the required version. Some features may not work correctly.", userMod.Name);
                    Mods.Add(userMod, new Mod(userMod, assembly));
                    Mods[userMod].Initialize();
                }
                else
                    log.ErrorFormat("[{0}] Minimum proxy version required required: {1}", userMod.Name, minProxyModVersion.ToString());
            }
            else
                log.ErrorFormat("[{0}] Proxy outdated, min required version: {1}", userMod.Name, modVersion);
        }

        internal void LoadModAssemblies()
        {
            foreach (var mod in Mods.Values)
            {
                if (mod.AssemblyRequestExtentionBase != null)
                {
                    foreach (var assembly in mod.AssemblyRequestExtentionBase.GetDependencyAssemblies())
                    {
                        if (ModDependencyAssemblies.ContainsKey(assembly.FullName))
                        {
                            log.WarnFormat("Skip Loading Dependency for mod [{0}]\nAssembly already loaded:\n{1}", mod.Information.Name, assembly.FullName);
                            continue;
                        }
                        log.InfoFormat("[{0}] Loading Dependency:\n{1}", mod.Information.Name, assembly.FullName);
                        ModDependencyAssemblies.Add(assembly.FullName, assembly);
                    }
                }
            }
        }

        public void Dispose()
        {
            foreach (var mod in Mods)
                mod.Value.Dispose();
        }
    }
}
