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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace IProxy.DataSerializing
{
    public class DWriter : BinaryWriter
    {
        public DWriter(Stream s) : base(s, Encoding.UTF8) { }

        public override void Write(short value)
        {
            base.Write(IPAddress.HostToNetworkOrder(value));
        }
        public override void Write(int value)
        {
            base.Write(IPAddress.HostToNetworkOrder(value));
        }
        public override void Write(long value)
        {
            base.Write(IPAddress.HostToNetworkOrder(value));
        }
        public override void Write(ushort value)
        {
            base.Write((ushort)IPAddress.HostToNetworkOrder((short)value));
        }
        public override void Write(uint value)
        {
            base.Write((uint)IPAddress.HostToNetworkOrder((int)value));
        }
        public override void Write(ulong value)
        {
            base.Write((ulong)IPAddress.HostToNetworkOrder((long)value));
        }
        public override void Write(float value)
        {
            var b = BitConverter.GetBytes(value);
            Array.Reverse(b);
            base.Write(b);
        }
        public override void Write(double value)
        {
            var b = BitConverter.GetBytes(value);
            Array.Reverse(b);
            base.Write(b);
        }

        public void WriteNullTerminatedString(string str)
        {
            Write(Encoding.UTF8.GetBytes(str));
            Write((byte)0);
        }

        public void WriteUTF(string str)
        {
            var data = Encoding.UTF8.GetBytes(str);
            Write((short)data.Length);
            base.Write(data);
        }

        public void Write32UTF(string str)
        {
            var data = Encoding.UTF8.GetBytes(str);
            Write((int)data.Length);
            base.Write(data);
        }
    }
}
