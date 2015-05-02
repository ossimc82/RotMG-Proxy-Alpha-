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
using IProxy.DataSerializing;
using System;
namespace IProxy.Networking.ClientPackets
{
    public class HelloPacket : ClientPacket
    {
        public string BuildVersion { get; set; }
        public int GameId { get; set; }
        public string GUID { get; set; }
        public int RandomInt1 { get; set; }
        public string Password { get; set; }
        public int RandomInt2 { get; set; }
        public string Secret { get; set; }
        public int KeyTime { get; set; }
        public byte[] Key { get; set; }
        public byte[] MapInfo { get; set; }
        public string obf1 { get; set; }
        public string obf2 { get; set; }
        public string obf3 { get; set; }
        public string obf4 { get; set; }
        public string obf5 { get; set; }

        public override PacketID ID
        {
            get { return PacketID.HELLO; }
        }

        public override Packet CreateInstance()
        {
            return new HelloPacket();
        }

        protected override void Read(DReader rdr)
        {
            BuildVersion = rdr.ReadUTF();
            GameId = rdr.ReadInt32();
            GUID = rdr.ReadUTF();
            RandomInt1 = rdr.ReadInt32(); //random int
            Password = rdr.ReadUTF();
            RandomInt2 = rdr.ReadInt32(); //random int
            Secret = rdr.ReadUTF();
            KeyTime = rdr.ReadInt32();
            Key = rdr.ReadBytes(rdr.ReadInt16());
            MapInfo = rdr.ReadBytes(rdr.ReadInt32());
            obf1 = rdr.ReadUTF();
            obf2 = rdr.ReadUTF();
            obf3 = rdr.ReadUTF();
            obf4 = rdr.ReadUTF();
            obf5 = rdr.ReadUTF();
        }

        protected override void Write(DWriter wtr)
        {
            wtr.WriteUTF(BuildVersion);
            wtr.Write(GameId);
            wtr.WriteUTF(GUID);
            wtr.Write(RandomInt1); //random int
            wtr.WriteUTF(Password);
            wtr.Write(RandomInt2); //random int
            wtr.WriteUTF(Secret);
            wtr.Write(KeyTime);
            wtr.Write((short)Key.Length);
            wtr.Write(Key);
            wtr.Write(MapInfo.Length);
            wtr.Write(MapInfo);
            wtr.WriteUTF(obf1);
            wtr.WriteUTF(obf2);
            wtr.WriteUTF(obf3);
            wtr.WriteUTF(obf4);
            wtr.WriteUTF(obf5);
        }
    }
}