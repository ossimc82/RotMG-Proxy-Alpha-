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
using System.Security.Cryptography;

namespace Proxy
{
    public class RC4
    {
        private byte[] m_State = new byte[256];

        public int X { get; set; }
        public int Y { get; set; }

        public byte[] State
        {
            get
            {
                byte[] buf = new byte[256];
                Array.Copy(this.m_State, buf, 256);
                return buf;
            }
            set
            {
                Array.Copy(value, this.m_State, 256);
            }
        }

        struct s
        {
            public int x; public int y;
            public byte[] state;
        }
        public object SaveState()
        {
            return new s()
            {
                x = X,
                y = Y,
                state = (byte[])State.Clone()
            };
        }
        public void LoadState(object o)
        {
            s s = (s)o;
            X = s.x;
            Y = s.y;
            State = s.state;
        }

        public RC4(byte[] key)
        {
            for (int i = 0; i < 256; i++)
            {
                this.m_State[i] = (byte)i;
            }

            this.X = 0;
            this.Y = 0;

            int index1 = 0;
            int index2 = 0;

            byte tmp;

            if (key == null || key.Length == 0)
            {
                throw new Exception();
            }

            for (int i = 0; i < 256; i++)
            {
                index2 = ((key[index1] & 0xff) + (this.m_State[i] & 0xff) + index2) & 0xff;

                tmp = this.m_State[i];
                this.m_State[i] = this.m_State[index2];
                this.m_State[index2] = tmp;

                index1 = (index1 + 1) % key.Length;
            }
        }

        public byte[] Crypt(byte[] buf)
        {
            int xorIndex;
            byte tmp;

            if (buf == null)
            {
                return null;
            }

            byte[] result = new byte[buf.Length];

            for (int i = 0; i < buf.Length; i++)
            {

                this.X = (this.X + 1) & 0xff;
                this.Y = ((this.m_State[this.X] & 0xff) + this.Y) & 0xff;

                tmp = this.m_State[this.X];
                this.m_State[this.X] = this.m_State[this.Y];
                this.m_State[this.Y] = tmp;

                xorIndex = ((this.m_State[this.X] & 0xff) + (this.m_State[this.Y] & 0xff)) & 0xff;
                result[i] = (byte)(buf[i] ^ this.m_State[xorIndex]);
            }

            return result;
        }
    }
}
