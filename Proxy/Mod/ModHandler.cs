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
using IProxy.Networking.ClientPackets;
using IProxy.Networking.ServerPackets;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Proxy
{
    public sealed class ModHandler : IDisposable, IEnumerable<Mod>
    {
        public const string PROXY_MOD_VERSION = "1.1.0";
        public const string MINIMUM_PROXYMOD_VERSION = "1.1.0";

        private Dictionary<IProxyMod, Mod> m_mods;
        private Dictionary<string, Assembly> m_modDependencyAssemblies;
        private Dictionary<string, Mod> m_registeredCommands;

        private readonly static ILog log = LogManager.GetLogger(typeof(ModHandler));

        public ModHandler()
        {
            m_mods = new Dictionary<IProxyMod, Mod>();
            m_modDependencyAssemblies = new Dictionary<string, Assembly>();
            m_registeredCommands = new Dictionary<string, Mod>();

            Singleton<ModHandler>.SetInstance(this);
            Singleton<Network>.Instance.OnSendToServer += ModHandler_OnSendToServer;
            Singleton<Network>.Instance.OnSendToClient += ModHandler_OnSendToClient;
            Singleton<Network>.Seal();
        }

        public Dictionary<string, Assembly> Assemblies { get { return m_modDependencyAssemblies; } }

        public int ModCount { get { return this.m_mods.Count; } }

        public Mod this[IProxyMod mod]
        {
            get { return m_mods[mod]; }
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
            foreach (var mod in m_mods.Values)
                mod.Disconnect();
        }

        internal bool OnServerPacketReceive(ref Packet packet)
        {
            bool ret = true;
            foreach (var mod in m_mods.Values.Where(_ => _.Enabled && _.PacketHandlerExtentionBase != null))
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

            if (packet is PlayerTextPacket)
            {
                string commandString = Utils.ChangePacketType<PlayerTextPacket>(packet).Text;
                if (commandString.StartsWith("/"))
                {
                    commandString = commandString.Remove(0, 1);
                    if (m_registeredCommands.ContainsKey(commandString.Split(' ')[0]))
                    {
                        string command = commandString.Split(' ')[0];
                        string[] args = commandString.Replace(command, String.Empty).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                        if (!m_registeredCommands[command].CommandManager.OnCommandGet(command, args)) return false;
                    }
                }
            }

            foreach (var mod in m_mods.Values.Where(_ => _.Enabled && _.PacketHandlerExtentionBase != null))
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
            if (m_mods.ContainsKey(mod))
                m_mods[mod].Disable();
        }

        public void EnableMod(IProxyMod mod)
        {
            if (m_mods.ContainsKey(mod))
                m_mods[mod].Enable();
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
                    m_mods.Add(userMod, new Mod(userMod, assembly));
                    IEnumerable<string> commands = new string[0];
                    m_mods[userMod].Initialize(ref commands);

                    foreach (var command in commands)
                    {
                        string commandStr = command.StartsWith("/") ? command.Remove(0, 1) : command;
                        if (m_registeredCommands.ContainsKey(commandStr))
                        {
                            log.WarnFormat("[ModHandler] Duplicated command: \"{0}\", ignoring.", commandStr);
                            continue;
                        }
                        m_registeredCommands.Add(commandStr, m_mods[userMod]);
                    }
                }
                else
                    log.ErrorFormat("[{0}] Minimum proxy version required: {1}", userMod.Name, minProxyModVersion.ToString());
            }
            else
                log.ErrorFormat("[{0}] Proxy outdated, min required version: {1}", userMod.Name, modVersion);
        }

        internal void LoadModAssemblies()
        {
            foreach (var mod in m_mods.Values)
            {
                if (mod.AssemblyRequestExtentionBase != null)
                {
                    foreach (var assembly in mod.AssemblyRequestExtentionBase.GetDependencyAssemblies())
                    {
                        if (m_modDependencyAssemblies.ContainsKey(assembly.FullName))
                        {
                            log.WarnFormat("Skip Loading Dependency for mod [{0}]\nAssembly already loaded:\n{1}", mod.Information.Name, assembly.FullName);
                            continue;
                        }
                        log.InfoFormat("[{0}] Loading Dependency:\n{1}", mod.Information.Name, assembly.FullName);
                        m_modDependencyAssemblies.Add(assembly.FullName, assembly);
                    }
                }
            }
        }

        public void Dispose()
        {
            foreach (var mod in m_mods)
                mod.Value.Dispose();
        }

        public IEnumerator<Mod> GetEnumerator()
        {
            return new ModHandlerEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new ModHandlerEnumerator();
        }

        public class ModHandlerEnumerator : IEnumerator<Mod>
        {
            private List<Mod> m_items;
            private int m_index;

            public ModHandlerEnumerator()
            {
                this.m_items = Singleton<ModHandler>.Instance.m_mods.Select(_ => _.Value).ToList<Mod>();
                this.m_index = -1;
            }

            public Mod Current
            {
                get { return this.m_items[this.m_index]; }
            }

            public void Dispose()
            {
            }

            object System.Collections.IEnumerator.Current
            {
                get { return this.m_items[this.m_index]; }
            }

            public bool MoveNext()
            {
                this.m_index++;
                return m_index < m_items.Count;
            }

            public void Reset()
            {
                this.m_index = -1;
            }
        }
    }
}
