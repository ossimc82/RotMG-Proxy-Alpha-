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
using IProxy.Networking.ServerPackets;
using RealmManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealmManager.realm
{
    public class World
    {
        private Dictionary<int, Entity> entities;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public string Name { get; private set; }
        public string LanguageString { get; private set; }
        public int Difficulty { get; private set; }
        public uint Seed { get; private set; }
        public int Background { get; private set; }
        public bool AllowTeleport { get; private set; }
        public bool ShowDisplays { get; private set; }
        public string[] ClientXML { get; private set; }
        public string[] ExtraXML { get; private set; }

        public World()
        {
            entities = new Dictionary<int, Entity>();
        }

        internal void UpdateWorld(MapInfoPacket mapInfo)
        {
            Width = mapInfo.Width;
            Height = mapInfo.Height;
            Name = mapInfo.Name;
            LanguageString = mapInfo.LanguageString;
            Seed = mapInfo.Seed;
            Background = mapInfo.Background;
            Difficulty = mapInfo.Difficulty;
            AllowTeleport = mapInfo.AllowTeleport;
            ShowDisplays = mapInfo.ShowDisplays;
            ClientXML = mapInfo.ClientXML;
            ExtraXML = mapInfo.ExtraXML;
        }

        internal void Reset()
        {
            Width = -1;
            Height = -1;
            Name = null;
            LanguageString = null;
            Seed = uint.MaxValue;
            Background = -1;
            Difficulty = -1;
            AllowTeleport = false;
            ShowDisplays = false;
            ClientXML = null;
            ExtraXML = null;
            entities.Clear();
        }

        public Entity GetEntity(int id, int defObjectType = -1)
        {
            Entity en;
            if(!entities.TryGetValue(id, out en) && defObjectType != -1)
                entities.Add(id, en = Entity.Resolve(defObjectType));
            return en;
        }
    }
}
