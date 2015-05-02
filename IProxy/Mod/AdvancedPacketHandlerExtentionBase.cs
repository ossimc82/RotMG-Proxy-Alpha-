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
    public abstract class AdvancedPacketHandlerExtentionBase : PacketHandlerExtentionBase
    {
        public delegate bool PacketReceive<T>(ref T packet) where T : Packet;
        public delegate bool GeneralPacketReceive<T>(ref Packet packet) where T : Packet;

        private Dictionary<Type, Delegate> m_hooks;

        public AdvancedPacketHandlerExtentionBase()
        {
            m_hooks = new Dictionary<Type, Delegate>();
            HookPackets();
        }

        protected abstract void HookPackets();

        public void ApplyPacketHook<T>(PacketReceive<T> callback) where T : Packet
        {
            if (m_hooks.ContainsKey(typeof(T))) throw new InvalidOperationException("Packet already bound to a callback");
            m_hooks.Add(typeof(T), callback);
        }

        public void ApplyGeneralPacketHook<T>(GeneralPacketReceive<T> callback) where T : Packet
        {
            if (m_hooks.ContainsKey(typeof(T))) throw new InvalidOperationException("Packet already bound to a callback");
            m_hooks.Add(typeof(T), callback);
        }

        public sealed override bool OnServerPacketReceived(ref Packet packet)
        {
            Delegate target;
            if (m_hooks.TryGetValue(packet.GetType(), out target))
                return (bool)target.Method.Invoke(target.Target, new object[] { packet });
            return base.OnServerPacketReceived(ref packet);
        }

        public sealed override bool OnClientPacketReceived(ref Packet packet)
        {
            Delegate target;
            if (m_hooks.TryGetValue(packet.GetType(), out target))
                return (bool)target.Method.Invoke(target.Target ?? this, new object[] { packet });
            return base.OnClientPacketReceived(ref packet);
        }
    }
}
