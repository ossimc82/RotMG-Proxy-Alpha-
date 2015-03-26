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
namespace IProxy.Networking.ServerPackets
{
    public class ShowEffectPacket : ServerPacket
    {
        public EffectType EffectType { get; set; }
        public int TargetId { get; set; }
        public Position PosA { get; set; }
        public Position PosB { get; set; }
        public ARGB Color { get; set; }

        public override PacketID ID
        {
            get { return PacketID.SHOW_EFFECT; }
        }

        public override Packet CreateInstance()
        {
            return new ShowEffectPacket();
        }

        protected override void Read(DReader rdr)
        {
            EffectType = (EffectType) rdr.ReadByte();
            TargetId = rdr.ReadInt32();
            PosA = Position.Read(rdr);
            PosB = Position.Read(rdr);
            Color = ARGB.Read(rdr);
        }

        protected override void Write(DWriter wtr)
        {
            wtr.Write((byte) EffectType);
            wtr.Write(TargetId);
            PosA.Write(wtr);
            PosB.Write(wtr);
            Color.Write(wtr);
        }
    }
}