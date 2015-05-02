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

// 
// Dear maintainer:
// 
// When I wrote this, only God and I understood what I was doing
// Now, God only knows
// Once you are done trying to 'optimize' this routine,
// and have realized what a terrible mistake that was,
// please increment the following counter as a warning
// to the next guy:
// 
// total_hours_wasted_here = 12
// 

using IProxy.Mod;
using IProxy.Networking;
using IProxy.Mod.WinForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proxy
{
    public sealed class Mod : IDisposable
    {
        private IProxyMod userMod;
        private IWinFormHost winFormHost;
        private Assembly assembly;

        private List<object> m_customInjects;

        public Mod(IProxyMod userMod, Assembly assembly)
        {
            this.m_customInjects = new List<object>();
            this.userMod = userMod;
            this.assembly = assembly;
            Enabled = true;
        }

        public PacketHandlerExtentionBase PacketHandlerExtentionBase { get; private set; }
        public AssemblyRequestExtentionBase AssemblyRequestExtentionBase { get; private set; }
        public WinFormProviderExtentionBase WinFormProviderExtentionBase { get; private set; }
        public ISettingsProvider Settings { get; private set; }
        public ICommandManager CommandManager { get; private set; }
        public IProxyMod Information { get { return this.userMod; } }

        public bool Enabled { get; private set; }

        internal void Initialize(ref IEnumerable<string> commands)
        {
            CreateCustomInjections();
            PacketHandlerExtentionBase = GetMemberOrCreateModInstance<PacketHandlerExtentionBase>();
            AssemblyRequestExtentionBase = GetMemberOrCreateModInstance<AssemblyRequestExtentionBase>();
            WinFormProviderExtentionBase = GetMemberOrCreateModInstance<WinFormProviderExtentionBase>();

            if (PacketHandlerExtentionBase is ICommandManager)
                CommandManager = (PacketHandlerExtentionBase as ICommandManager);
            else CommandManager = GetMemberOrCreateModInstance<ICommandManager>();

            if (CommandManager != null) commands = CommandManager.RegisterCommands();

            if ((winFormHost = GetMemberOrCreateModInstance<IWinFormHost>()) != null && WinFormProviderExtentionBase != null && winFormHost.RunOnLoad())
            {
                WinFormProviderExtentionBase.CreateSingletonForProvider();
                WinFormProviderExtentionBase.SetNextForm(winFormHost.GetDefaultForm());
                WinFormProviderExtentionBase.FormThread.Start();
            }

            if ((Settings = GetMemberOrCreateModInstance<ISettingsProvider>()) != null)
                Settings.Register(System.Runtime.InteropServices.Marshal.GenerateGuidForType(Settings.GetType()).ToString());
        }

        public void Disconnect()
        {
            DisconnectIfNotNull<PacketHandlerExtentionBase>(PacketHandlerExtentionBase);
            DisconnectIfNotNull<AssemblyRequestExtentionBase>(AssemblyRequestExtentionBase);
        }

        private void DisconnectIfNotNull<T>(ProxyExtentionBase instance) where T : ProxyExtentionBase, IDisposable
        {
            if (instance != null)
            {
                instance.OnDisconnect();
                if (instance.DisposeAfterDisconnect())
                {
                    instance.Dispose();
                    instance = GetMemberOrCreateModInstance<T>();
                }
            }
        }

        private T GetMemberOrCreateModInstance<T>() where T : class
        {
            Type classType;

            foreach (var obj in m_customInjects)
            {
                MemberInfo targetMember = null;
                foreach (var member in obj.GetType().GetMembers())
                {
                    var attr = member.GetCustomAttribute<IProxyModMemberAttribute>();
                    if (attr != null)
                    {
                        if (attr.TargetType == typeof(T))
                        {
                            return (obj.GetType().GetField(member.Name).GetValue(obj) as T);
                        }
                    }
                }

                if (targetMember != null)
                {
                    Console.WriteLine(targetMember.Name);
                }
            }


            classType = (from type in assembly.GetTypes()
                            where typeof(T).IsAssignableFrom(type)
                            select type).FirstOrDefault();

            if (classType != null)
                return Activator.CreateInstance(classType) as T;
            return null;
        }

        private void CreateCustomInjections()
        {
            var classTypes = (from type in assembly.GetTypes()
                              where type.GetCustomAttribute<IProxyInjectAttribute>() != null && !type.IsAbstract
                              select type);

            foreach (var classType in classTypes)
                m_customInjects.Add(Activator.CreateInstance(classType));
        }

        public void Disable()
        {
            Enabled = false;
        }

        public void Enable()
        {
            Enabled = true;
        }

        public void Dispose()
        {
            DisposeIfNotNull(Settings);
            DisposeIfNotNull(PacketHandlerExtentionBase);
            DisposeIfNotNull(AssemblyRequestExtentionBase);
        }

        private void DisposeIfNotNull(IDisposable target)
        {
            if (target == null) return;
            target.Dispose();
        }
    }
}
