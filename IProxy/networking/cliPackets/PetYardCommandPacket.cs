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
namespace IProxy.Networking.ClientPackets
{
    public class PetYardCommandPacket : ClientPacket
    {
        public const int UPGRADE_PET_YARD = 1;
        public const int FEED_PET = 2;
        public const int FUSE_PET = 3;

        public byte CommandId { get; set; }
        public int PetId1 { get; set; }
        public int PetId2 { get; set; }
        public int ObjectId { get; set; }
        public ObjectSlot ObjectSlot { get; set; }
        public CurrencyType Currency { get; set; }

        public override PacketID ID
        {
            get { return PacketID.PETYARDCOMMAND; }
        }

        public override Packet CreateInstance()
        {
            return new PetYardCommandPacket();
        }

        protected override void Read(DReader rdr)
        {
            CommandId = rdr.ReadByte();
            PetId1 = rdr.ReadInt32();
            PetId2 = rdr.ReadInt32();
            ObjectId = rdr.ReadInt32();
            ObjectSlot = ObjectSlot.Read(rdr);
            Currency = (CurrencyType)rdr.ReadByte();
        }

        protected override void Write(DWriter wtr)
        {
            wtr.Write(CommandId);
            wtr.Write(PetId1);
            wtr.Write(PetId2);
            wtr.Write(ObjectId);
            ObjectSlot.Write(wtr);
            wtr.Write((byte)Currency);
        }
    }
}
