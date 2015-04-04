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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IProxy
{
    public enum CurrencyType
    {
        Gold = 0,
        Fame = 1,
        GuildFame = 2,
        FortuneTokens = 3
    }

    public struct StatsType
    {
        public readonly static StatsType MaximumHP = 0;
        public readonly static StatsType HP = 1;
        public readonly static StatsType Size = 2;
        public readonly static StatsType MaximumMP = 3;
        public readonly static StatsType MP = 4;
        public readonly static StatsType ExperienceGoal = 5;
        public readonly static StatsType Experience = 6;
        public readonly static StatsType Level = 7;
        public readonly static StatsType Inventory0 = 8;
        public readonly static StatsType Inventory1 = 9;
        public readonly static StatsType Inventory2 = 10;
        public readonly static StatsType Inventory3 = 11;
        public readonly static StatsType Inventory4 = 12;
        public readonly static StatsType Inventory5 = 13;
        public readonly static StatsType Inventory6 = 14;
        public readonly static StatsType Inventory7 = 15;
        public readonly static StatsType Inventory8 = 16;
        public readonly static StatsType Inventory9 = 17;
        public readonly static StatsType Inventory10 = 18;
        public readonly static StatsType Inventory11 = 19;
        public readonly static StatsType Attack = 20;
        public readonly static StatsType Defense = 21;
        public readonly static StatsType Speed = 22;
        public readonly static StatsType Vitality = 26;
        public readonly static StatsType Wisdom = 27;
        public readonly static StatsType Dexterity = 28;
        public readonly static StatsType Effects = 29;
        public readonly static StatsType Stars = 30;
        public readonly static StatsType Name = 31; //Is UTF
        public readonly static StatsType Texture1 = 32;
        public readonly static StatsType Texture2 = 33;
        public readonly static StatsType MerchantMerchandiseType = 34;
        public readonly static StatsType Credits = 35;
        public readonly static StatsType SellablePrice = 36;
        public readonly static StatsType PortalUsable = 37;
        public readonly static StatsType AccountId = 38; //Is UTF
        public readonly static StatsType CurrentFame = 39;
        public readonly static StatsType SellablePriceCurrency = 40;
        public readonly static StatsType ObjectConnection = 41;
        /*
         * Mask :F0F0F0F0
         * each byte > type
         * 0:Dot
         * 1:ushortLine
         * 2:L
         * 3:Line
         * 4:T
         * 5:Cross
         * 0x21222112
        */
        public readonly static StatsType MerchantRemainingCount = 42;
        public readonly static StatsType MerchantRemainingMinute = 43;
        public readonly static StatsType MerchantDiscount = 44;
        public readonly static StatsType SellableRankRequirement = 45;
        public readonly static StatsType HPBoost = 46;
        public readonly static StatsType MPBoost = 47;
        public readonly static StatsType AttackBonus = 48;
        public readonly static StatsType DefenseBonus = 49;
        public readonly static StatsType SpeedBonus = 50;
        public readonly static StatsType VitalityBonus = 51;
        public readonly static StatsType WisdomBonus = 52;
        public readonly static StatsType DexterityBonus = 53;
        public readonly static StatsType OwnerAccountId = 54; //Is UTF
        public readonly static StatsType NameChangerStar = 55;
        public readonly static StatsType NameChosen = 56;
        public readonly static StatsType Fame = 57;
        public readonly static StatsType FameGoal = 58;
        public readonly static StatsType Glowing = 59;
        public readonly static StatsType SinkOffset = 60;
        public readonly static StatsType AltTextureIndex = 61;
        public readonly static StatsType Guild = 62; //Is UTF
        public readonly static StatsType GuildRank = 63;
        public readonly static StatsType OxygenBar = 64;
        public readonly static StatsType XpBoosterActive = 65;
        public readonly static StatsType XpBoosterTime = 66;
        public readonly static StatsType LootDropBoostTimer = 67;
        public readonly static StatsType LootTierBoostTimer = 68;
        public readonly static StatsType HealStackCount = 69;
        public readonly static StatsType MagicStackCount = 70;
        public readonly static StatsType Backpack0 = 71;
        public readonly static StatsType Backpack1 = 72;
        public readonly static StatsType Backpack2 = 73;
        public readonly static StatsType Backpack3 = 74;
        public readonly static StatsType Backpack4 = 75;
        public readonly static StatsType Backpack5 = 76;
        public readonly static StatsType Backpack6 = 77;
        public readonly static StatsType Backpack7 = 78;
        public readonly static StatsType Has_Backpack = 79;
        public readonly static StatsType Skin = 80;
        public readonly static StatsType PetId = 81;
        public readonly static StatsType PetSkin = 82; //Is UTF
        public readonly static StatsType PetType = 83;
        public readonly static StatsType PetRarity = 84;
        public readonly static StatsType PetMaximumLevel = 85;
        public readonly static StatsType PetNothing = 86; //This does do nothing in the client
        public readonly static StatsType PetPoints0 = 87;
        public readonly static StatsType PetPoints1 = 88;
        public readonly static StatsType PetPoints2 = 89;
        public readonly static StatsType PetLevel0 = 90;
        public readonly static StatsType PetLevel1 = 91;
        public readonly static StatsType PetLevel2 = 92;
        public readonly static StatsType PetAbilityType0 = 93;
        public readonly static StatsType PetAbilityType1 = 94;
        public readonly static StatsType PetAbilityType2 = 95;
        public readonly static StatsType _04I = 96;
        //case StatData._-04I:
        //   param1.condition_[ConditionEffect._-KL]=_loc8_;
        //   break;
        public readonly static StatsType FortuneTokens = 97;

        private byte m_type;

        private StatsType(byte type)
        {
            this.m_type = type;
        }

        public bool IsUTF()
        {
            if (this == StatsType.Name || this == StatsType.AccountId || this == StatsType.OwnerAccountId
               || this == StatsType.Guild || this == StatsType.PetSkin)
                    return true;
            return false;
        }

        public static implicit operator StatsType(int type)
        {
            if (type > byte.MaxValue) throw new Exception("Not a valid StatData number.");
            return new StatsType((byte)type);
        }

        public static implicit operator StatsType(byte type)
        {
            return new StatsType(type);
        }

        public static bool operator ==(StatsType type, int id)
        {
            if (id > byte.MaxValue) throw new Exception("Not a valid StatData number.");
            return type.m_type == (byte)id;
        }

        public static bool operator ==(StatsType type, byte id)
        {
            return type.m_type == id;
        }

        public static bool operator !=(StatsType type, int id)
        {
            if (id > byte.MaxValue) throw new Exception("Not a valid StatData number.");
            return type.m_type != (byte)id;
        }

        public static bool operator !=(StatsType type, byte id)
        {
            return type.m_type != id;
        }

        public static bool operator ==(StatsType type, StatsType id)
        {
            return type.m_type == id.m_type;
        }

        public static bool operator !=(StatsType type, StatsType id)
        {
            return type.m_type != id.m_type;
        }

        public static implicit operator int(StatsType type)
        {
            return type.m_type;
        }

        public static implicit operator byte(StatsType type)
        {
            return type.m_type;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is StatsType)) return false;
            return this == (StatsType)obj;
        }
        public override string ToString()
        {
            return m_type.ToString();
        }
    }

    public struct BitmapData
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Bytes { get; set; }

        public static BitmapData Read(DReader rdr)
        {
            BitmapData ret = new BitmapData();
            ret.Width = rdr.ReadInt32();
            ret.Height = rdr.ReadInt32();
            ret.Bytes = new byte[ret.Width * ret.Height * 4];
            ret.Bytes = rdr.ReadBytes(ret.Bytes.Length);
            return ret;
        }

        public void Write(DWriter wtr)
        {
            wtr.Write(Width);
            wtr.Write(Height);
            wtr.Write(Bytes);
        }
    }

    public struct IntPointComparer : IEqualityComparer<IntPoint>
    {
        public bool Equals(IntPoint x, IntPoint y)
        {
            return x.X == y.X && x.Y == y.Y;
        }

        public int GetHashCode(IntPoint obj)
        {
            return obj.X * 23 << 16 + obj.Y * 17;
        }
    }

    public struct IntPoint
    {
        public int X;
        public int Y;

        public IntPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public struct TradeItem
    {
        public bool Included;
        public int Item;
        public int SlotType;
        public bool Tradeable;

        public static TradeItem Read(DReader rdr)
        {
            TradeItem ret = new TradeItem();
            ret.Item = rdr.ReadInt32();
            ret.SlotType = rdr.ReadInt32();
            ret.Tradeable = rdr.ReadBoolean();
            ret.Included = rdr.ReadBoolean();
            return ret;
        }

        public void Write(DWriter wtr)
        {
            wtr.Write(Item);
            wtr.Write(SlotType);
            wtr.Write(Tradeable);
            wtr.Write(Included);
        }
    }

    public enum EffectType
    {
        Potion = 1,
        Teleport = 2,
        Stream = 3,
        Throw = 4,
        AreaBlast = 5, //radius=pos1.x
        Dead = 6,
        Trail = 7,
        Diffuse = 8, //radius=dist(pos1,pos2)
        Flow = 9,
        Trap = 10, //radius=pos1.x
        Lightning = 11, //particleSize=pos2.x
        Concentrate = 12, //radius=dist(pos1,pos2)
        BlastWave = 13, //origin=pos1, radius = pos2.x
        Earthquake = 14,
        Flashing = 15, //period=pos1.x, numCycles=pos1.y
        BeachBall = 16,
        ElectricBolts = 17, //If a pet paralyzes a monster
        ElectricFlashing = 18, //If a monster got paralyzed from a electric pet
        SavageEffect = 19 //If a pet is standing still (this white particles)
    }

    public struct ARGB
    {
        public byte A;
        public byte B;
        public byte G;
        public byte R;

        public ARGB(uint argb)
        {
            A = (byte)((argb & 0xff000000) >> 24);
            R = (byte)((argb & 0x00ff0000) >> 16);
            G = (byte)((argb & 0x0000ff00) >> 8);
            B = (byte)((argb & 0x000000ff) >> 0);
        }

        public ARGB(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public static ARGB Read(DReader rdr)
        {
            ARGB ret = new ARGB();
            ret.A = rdr.ReadByte();
            ret.R = rdr.ReadByte();
            ret.G = rdr.ReadByte();
            ret.B = rdr.ReadByte();
            return ret;
        }

        public void Write(DWriter wtr)
        {
            wtr.Write(A);
            wtr.Write(R);
            wtr.Write(G);
            wtr.Write(B);
        }
    }

    public struct ObjectSlot
    {
        public int ObjectId;
        public ushort ObjectType;
        public byte SlotId;

        public static ObjectSlot Read(DReader rdr)
        {
            ObjectSlot ret = new ObjectSlot();
            ret.ObjectId = rdr.ReadInt32();
            ret.SlotId = rdr.ReadByte();
            ret.ObjectType = (ushort)rdr.ReadInt16();
            return ret;
        }

        public void Write(DWriter wtr)
        {
            wtr.Write(ObjectId);
            wtr.Write(SlotId);
            wtr.Write(ObjectType);
        }

        public override string ToString()
        {
            return string.Format("{{ObjectId: {0}, SlotId: {1}, ObjectType: {2}}}", ObjectId, SlotId, ObjectType);
        }
    }

    public struct TimedPosition
    {
        public Position Position;
        public int Time;

        public static TimedPosition Read(DReader rdr)
        {
            TimedPosition ret = new TimedPosition();
            ret.Time = rdr.ReadInt32();
            ret.Position = Position.Read(rdr);
            return ret;
        }

        public void Write(DWriter wtr)
        {
            wtr.Write(Time);
            Position.Write(wtr);
        }

        public override string ToString()
        {
            return string.Format("{{Time: {0}, Position: {1}}}", Time, Position);
        }
    }

    public struct Position
    {
        public float X;
        public float Y;

        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Position Read(DReader rdr)
        {
            Position ret = new Position();
            ret.X = rdr.ReadSingle();
            ret.Y = rdr.ReadSingle();
            return ret;
        }

        public void Write(DWriter wtr)
        {
            wtr.Write(X);
            wtr.Write(Y);
        }

        public override string ToString()
        {
            return string.Format("{{X: {0}, Y: {1}}}", X, Y);
        }
    }

    public struct ObjectDef
    {
        public ushort ObjectType;
        public ObjectStats Stats;

        public static ObjectDef Read(DReader rdr)
        {
            ObjectDef ret = new ObjectDef();
            ret.ObjectType = (ushort)rdr.ReadInt16();
            ret.Stats = ObjectStats.Read(rdr);
            return ret;
        }

        public void Write(DWriter wtr)
        {
            wtr.Write(ObjectType);
            Stats.Write(wtr);
        }
    }

    public struct ObjectStats
    {
        public int Id;
        public Position Position;
        public KeyValuePair<StatsType, object>[] Stats;

        public static ObjectStats Read(DReader rdr)
        {
            ObjectStats ret = new ObjectStats();
            ret.Id = rdr.ReadInt32();
            ret.Position = Position.Read(rdr);
            ret.Stats = new KeyValuePair<StatsType, object>[rdr.ReadInt16()];
            for (int i = 0; i < ret.Stats.Length; i++)
            {
                StatsType type = (StatsType)rdr.ReadByte();
                if (type.IsUTF())
                    ret.Stats[i] = new KeyValuePair<StatsType, object>(type, rdr.ReadUTF());
                else
                    ret.Stats[i] = new KeyValuePair<StatsType, object>(type, rdr.ReadInt32());
            }
            return ret;
        }

        public void Write(DWriter wtr)
        {
            try
            {
                wtr.Write(Id);
                Position.Write(wtr);
                wtr.Write((ushort)Stats.Length);
                foreach (KeyValuePair<StatsType, object> i in Stats)
                {
                    wtr.Write((byte)i.Key);
                    if (i.Key.IsUTF() && i.Value != null) wtr.WriteUTF(i.Value.ToString());
                    else wtr.Write((int)i.Value);
                }
            }
            catch (Exception) { }
        }
    }
}