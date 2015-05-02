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
using IProxy.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IProxy.Mod
{
    [IProxyInject]
    public abstract class AdvancedPacketHandlerWithAdvancedCommandManagerExtentionBase : ProxyExtentionBase
    {
        [IProxyModMember(typeof(PacketHandlerExtentionBase))]
        public AdvancedPacketHandlerExtentionBase PacketHandler;
        [IProxyModMember(typeof(ICommandManager))]
        public AdvancedCommandManager CommandManager;

        public AdvancedPacketHandlerWithAdvancedCommandManagerExtentionBase()
        {
            this.PacketHandler = new AdvancedPacketHandlerImplementation();
            this.CommandManager = new AdvancedCommandManagerImplementation();

            HookPackets();
            HookCommands();
        }

        protected abstract void HookPackets();
        protected abstract void HookCommands();

        public void ApplyPacketHook<T>(AdvancedPacketHandlerExtentionBase.PacketReceive<T> callback) where T : Packet
        {
            this.PacketHandler.ApplyPacketHook<T>(callback);
        }

        public void ApplyPacketHook<T>(AdvancedPacketHandlerExtentionBase.GeneralPacketReceive<T> callback) where T : Packet
        {
            this.PacketHandler.ApplyGeneralPacketHook<T>(callback);
        }

        public void ApplyCommandHook(string command, AdvancedCommandManager.Command callback)
        {
            this.CommandManager.ApplyCommandHook(command, callback);
        }

        private class AdvancedPacketHandlerImplementation : AdvancedPacketHandlerExtentionBase
        {
            public AdvancedPacketHandlerImplementation() { }

            protected override void HookPackets() { }
        }
        private class AdvancedCommandManagerImplementation : AdvancedCommandManager
        {
            public override void HookCommands() { }
        }
    }
}
