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
using IProxy.common.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealmManager.Entities
{
    public class Entity
    {
        public Entity(int objType)
        {
            this.ObjectType = objType;
        }

        public int ObjectType { get; private set; }
        public int Id { get; internal set; }
        public int Size { get; private set; }
        public string Name { get; private set; }
        public ConditionEffects ConditionEffects { get; private set; }
        public Position Position { get; internal set; }

        internal virtual void ImportStats(StatsType stat, object val)
        {
            if (stat == StatsType.Name) Name = (string)val;
            else if (stat == StatsType.Size) Size = (int)val;
            else if (stat == StatsType.Effects) ConditionEffects = (ConditionEffects)(int)val;
        }

        internal void ImportStats(ObjectStats stats)
        {
            Id = stats.Id;
            Position = stats.Position;
            foreach (var i in stats.Stats)
                ImportStats(i.Key, i.Value);
        }

        internal static Entity Resolve(int objType)
        {
            ObjectDesc desc;
            if (Singleton<XmlData>.Instance.ObjectDescs.TryGetValue((ushort)objType, out desc))
            {
                switch (desc.Class)
                {
                    case "Character":
                        return new Character(objType);
                    case "Player":
                        return new Player(objType);
                }
            }
            return new Entity(objType);
        }
    }
}
