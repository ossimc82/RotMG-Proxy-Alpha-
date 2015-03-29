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
using IProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealmManager.entities;
using IProxy.common.data;

namespace RealmManager.Entities
{
    public class Player : Character, IContainer
    {
        private Stats m_statValues;
        private Stats m_statBoosts;

        public Player(int objType)
            : base(objType)
        {
            m_statBoosts = new Stats(8);
            m_statValues = new Stats(8);

            Inventory = new Inventory(this);
            Backpack = new Inventory(this, new Item[8]);
        }

        public Player() : this(-1) { }

        public string AccountId { get; private set; }
        public int Experience { get; private set; }
        public int ExperienceGoal { get; private set; }
        public int Level { get; private set; }
        public int CurrentFame { get; private set; }
        public int Fame { get; private set; }
        public int FameGoal { get; private set; }
        public int Stars { get; private set; }
        public string GuildName { get; private set; }
        public int GuildRank { get; private set; }
        public int Credits { get; private set; }
        public int FortuneTokens { get; private set; }
        public bool NameChosen { get; private set; }
        public int Texture1 { get; private set; }
        public int Texture2 { get; private set; }
        public bool Glowing { get; private set; }
        public Inventory Inventory { get; private set; }
        public Stats StatValues { get { return m_statValues; } }
        public Stats StatBoosts { get { return m_statBoosts; } }
        public bool HasBackpack { get; private set; }
        public Inventory Backpack { get; private set; }
        public int MP { get; private set; }
        public int Skin { get; private set; }
        public int HealthPotions { get; private set; }
        public int MagicPotions { get; private set; }
        public int OxygenBar { get; private set; }
        public bool XpBoosted { get; private set; }
        public int XpBoostTimeLeft { get; private set; }
        public int LootDropBoostTimeLeft { get; private set; }
        public int LootTierBoostTimeLeft { get; private set; }

        internal override void ImportStats(StatsType stat, object val)
        {
            base.ImportStats(stat, val);

            if (stat == StatsType.AccountId) AccountId = (string)val;
            else if (stat == StatsType.Experience) Experience = (int)val;
            else if (stat == StatsType.ExperienceGoal) ExperienceGoal = (int)val;
            else if (stat == StatsType.Level) Level = (int)val;
            else if (stat == StatsType.CurrentFame) CurrentFame = (int)val;
            else if (stat == StatsType.Fame) Fame = (int)val;
            else if (stat == StatsType.FameGoal) FameGoal = (int)val;
            else if (stat == StatsType.Stars) Stars = (int)val;
            else if (stat == StatsType.Guild) GuildName = (string)val;
            else if (stat == StatsType.GuildRank) GuildRank = (int)val;
            else if (stat == StatsType.Credits) Credits = (int)val;
            else if (stat == StatsType.FortuneTokens) FortuneTokens = (int)val;
            else if (stat == StatsType.NameChosen) NameChosen = (int)val != 0;
            else if (stat == StatsType.Texture1) Texture1 = (int)val;
            else if (stat == StatsType.Texture2) Texture2 = (int)val;
            else if (stat == StatsType.Glowing) Glowing = (int)val > -1;
            else if (stat == StatsType.MP) MP = (int)val;

            else if (stat == StatsType.Inventory0) Inventory[0] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Inventory1) Inventory[1] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Inventory2) Inventory[2] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Inventory3) Inventory[3] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Inventory4) Inventory[4] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Inventory5) Inventory[5] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Inventory6) Inventory[6] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Inventory7) Inventory[7] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Inventory8) Inventory[8] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Inventory9) Inventory[9] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Inventory10) Inventory[10] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Inventory11) Inventory[11] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];

            else if (stat == StatsType.HPBoost) m_statBoosts[Stat.HP] = (int)val;
            else if (stat == StatsType.MPBoost) m_statBoosts[Stat.MP] = (int)val;
            else if (stat == StatsType.AttackBonus) m_statBoosts[Stat.ATT] = (int)val;
            else if (stat == StatsType.DefenseBonus) m_statBoosts[Stat.DEF] = (int)val;
            else if (stat == StatsType.SpeedBonus) m_statBoosts[Stat.SPD] = (int)val;
            else if (stat == StatsType.VitalityBonus) m_statBoosts[Stat.VIT] = (int)val;
            else if (stat == StatsType.WisdomBonus) m_statBoosts[Stat.WIS] = (int)val;
            else if (stat == StatsType.DexterityBonus) m_statBoosts[Stat.DEX] = (int)val;

            else if (stat == StatsType.MaximumHP) m_statValues[Stat.MAX_HP] = (int)val;
            else if (stat == StatsType.MaximumMP) m_statValues[Stat.MAX_MP] = (int)val;
            else if (stat == StatsType.Attack) m_statValues[Stat.ATT] = (int)val;
            else if (stat == StatsType.Defense) m_statValues[Stat.DEF] = (int)val;
            else if (stat == StatsType.Speed) m_statValues[Stat.SPD] = (int)val;
            else if (stat == StatsType.Vitality) m_statValues[Stat.VIT] = (int)val;
            else if (stat == StatsType.Wisdom) m_statValues[Stat.WIS] = (int)val;
            else if (stat == StatsType.Dexterity) m_statValues[Stat.DEX] = (int)val;

            else if (stat == StatsType.Has_Backpack) HasBackpack = (int)val > 0;

            else if (stat == StatsType.Backpack0) Backpack[0] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Backpack1) Backpack[1] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Backpack2) Backpack[2] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Backpack3) Backpack[3] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Backpack4) Backpack[4] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Backpack5) Backpack[5] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Backpack6) Backpack[6] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];
            else if (stat == StatsType.Backpack7) Backpack[7] = (int)val == -1 ? null : Singleton<XmlData>.Instance.Items[(ushort)(int)val];

            else if (stat == StatsType.Skin) Skin = (int)val;
            else if (stat == StatsType.HealStackCount) HealthPotions = (int)val;
            else if (stat == StatsType.MagicStackCount) MagicPotions = (int)val;

            else if (stat == StatsType.OxygenBar) OxygenBar = (int)val;

            else if (stat == StatsType.XpBoosterActive) XpBoosted = (int)val > 0;
            else if (stat == StatsType.XpBoosterTime) XpBoostTimeLeft = (int)val;
            else if (stat == StatsType.LootDropBoostTimer) LootDropBoostTimeLeft = (int)val;
            else if (stat == StatsType.LootTierBoostTimer) LootTierBoostTimeLeft = (int)val;
        }
    }
}
