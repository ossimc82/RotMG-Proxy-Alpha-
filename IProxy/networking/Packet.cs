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
#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using IProxy.Networking.ClientPackets;
using IProxy.Networking.ServerPackets;
using IProxy.DataSerializing;

#endregion

namespace IProxy.Networking
{
    public abstract class Packet
    {
        public static Dictionary<PacketID, Packet> Packets = new Dictionary<PacketID, Packet>();
        public static Dictionary<PacketID, Packet> ServerPackets = new Dictionary<PacketID, Packet>();
        public static Dictionary<PacketID, Packet> ClientPackets = new Dictionary<PacketID, Packet>();

        static Packet()
        {
            foreach (Type i in typeof (Packet).Assembly.GetTypes())
                if (typeof (Packet).IsAssignableFrom(i) && !i.IsAbstract)
                {
                    Packet pkt = (Packet) Activator.CreateInstance(i);
                    if (!Packets.ContainsKey(pkt.ID))
                        Packets.Add(pkt.ID, pkt);
                }

            foreach (Type i in typeof(Packet).Assembly.GetTypes())
                if (typeof(Packet).IsAssignableFrom(i) && !i.IsAbstract)
                {
                    Packet pkt = (Packet)Activator.CreateInstance(i);
                    if (!ServerPackets.ContainsKey(pkt.ID))
                        if(pkt is ServerPacket)
                            ServerPackets.Add(pkt.ID, pkt);
                }

            foreach (Type i in typeof(Packet).Assembly.GetTypes())
                if (typeof(Packet).IsAssignableFrom(i) && !i.IsAbstract)
                {
                    Packet pkt = (Packet)Activator.CreateInstance(i);
                    if (!ClientPackets.ContainsKey(pkt.ID))
                        if(pkt is ClientPacket)
                            ClientPackets.Add(pkt.ID, pkt);
                }
        }

        public abstract PacketID ID { get; }
        public abstract Packet CreateInstance();

        public void Read(byte[] body, int len)
        {
            DReader rdr = new DReader(new MemoryStream(body, 0, len));
            Read(rdr);
        }

        public int Write(ref byte[] buff)
        {
            DWriter wtr = new DWriter(new MemoryStream());
            Write(wtr);
            buff = (wtr.BaseStream as MemoryStream).ToArray();
            return (int)wtr.BaseStream.Length;
        }

        protected abstract void Read(DReader rdr);
        protected abstract void Write(DWriter wtr);

        public override string ToString()
        {
            //StringBuilder ret = new StringBuilder(new string('-', Console.BufferWidth) + this.ID + " received: \r\n");
            //PropertyInfo[] arr = GetType().GetProperties();
            //for (int i = 0; i < arr.Length; i++)
            //{
            //    if (arr[i].Name == "ID") continue;
            //    ret.AppendFormat("{0}: {1}\r\n", arr[i].Name, arr[i].GetValue(this, null));
            //}
            //ret.Append(new string('-', Console.BufferWidth));
            //return ret.ToString();
            StringBuilder ret = new StringBuilder("{");
            PropertyInfo[] arr = GetType().GetProperties();
            for (int i = 0; i < arr.Length; i++)
            {
                if (i != 0) ret.Append(",\n");
                ret.AppendFormat("{0}: {1}", arr[i].Name, arr[i].GetValue(this, null));
            }
            ret.Append("}");
            return ret.ToString();
        }
    }

    public class NopPacket : Packet
    {
        public override PacketID ID { get { return (PacketID)255; } }
        public override Packet CreateInstance() { return new NopPacket(); }
        protected override void Read(DReader rdr) { }
        protected override void Write(DWriter wtr) { }
    }
}