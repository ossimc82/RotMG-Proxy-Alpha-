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

using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;

#endregion

namespace IProxy.common.data
{
    public class XmlData
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(XmlData));

        private readonly Dictionary<string, ushort> id2type_obj;
        private readonly Dictionary<string, ushort> id2type_tile;
        private readonly Dictionary<ushort, Item> items;
        private readonly Dictionary<ushort, ObjectDesc> objDescs;
        private readonly Dictionary<ushort, PortalDesc> portals;
        private readonly Dictionary<ushort, TileDesc> tiles;
        private readonly Dictionary<ushort, XElement> type2elem_obj;
        private readonly Dictionary<ushort, XElement> type2elem_tile;
        private readonly Dictionary<string, PetSkin> id2pet_skin;
        private readonly Dictionary<ushort, PetStruct> type2pet;
        private readonly Dictionary<ushort, string> type2id_obj;
        private readonly Dictionary<ushort, string> type2id_tile;

        public XmlData()
        {
            ObjectTypeToElement = new ReadOnlyDictionary<ushort, XElement>(
                type2elem_obj = new Dictionary<ushort, XElement>());

            ObjectTypeToId = new ReadOnlyDictionary<ushort, string>(
                type2id_obj = new Dictionary<ushort, string>());
            IdToObjectType = new ReadOnlyDictionary<string, ushort>(
                id2type_obj = new Dictionary<string, ushort>(StringComparer.InvariantCultureIgnoreCase));

            TileTypeToElement = new ReadOnlyDictionary<ushort, XElement>(
                type2elem_tile = new Dictionary<ushort, XElement>());

            TileTypeToId = new ReadOnlyDictionary<ushort, string>(
                type2id_tile = new Dictionary<ushort, string>());
            IdToTileType = new ReadOnlyDictionary<string, ushort>(
                id2type_tile = new Dictionary<string, ushort>(StringComparer.InvariantCultureIgnoreCase));

            Tiles = new ReadOnlyDictionary<ushort, TileDesc>(
                tiles = new Dictionary<ushort, TileDesc>());
            Items = new ReadOnlyDictionary<ushort, Item>(
                items = new Dictionary<ushort, Item>());
            ObjectDescs = new ReadOnlyDictionary<ushort, ObjectDesc>(
                objDescs = new Dictionary<ushort, ObjectDesc>());
            Portals = new ReadOnlyDictionary<ushort, PortalDesc>(
                portals = new Dictionary<ushort, PortalDesc>());
            TypeToPet = new ReadOnlyDictionary<ushort, PetStruct>(
                type2pet = new Dictionary<ushort, PetStruct>());
            IdToPetSkin = new ReadOnlyDictionary<string, PetSkin>(
                id2pet_skin = new Dictionary<string, PetSkin>());


            string basePath = Path.Combine(AssemblyDirectory, "common\\data");
            log.InfoFormat("Loading game data from '{0}'...", basePath);
            string[] xmls = Directory.EnumerateFiles(basePath, "*.xml", SearchOption.AllDirectories).ToArray();
            for (int i = 0; i < xmls.Length; i++)
            {
                using (Stream stream = File.OpenRead(xmls[i]))
                    ProcessXml(XElement.Load(stream));
            }
            log.Info("Finish loading game data.");
            log.InfoFormat("{0} Items", items.Count);
            log.InfoFormat("{0} Tiles", tiles.Count);
            log.InfoFormat("{0} Objects", objDescs.Count);
        }

        private static string AssemblyDirectory
        {
            get { return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location); }
        }


        public IDictionary<ushort, XElement> ObjectTypeToElement { get; private set; }

        public IDictionary<ushort, string> ObjectTypeToId { get; private set; }
        public IDictionary<string, ushort> IdToObjectType { get; private set; }

        public IDictionary<ushort, XElement> TileTypeToElement { get; private set; }

        public IDictionary<ushort, string> TileTypeToId { get; private set; }
        public IDictionary<string, ushort> IdToTileType { get; private set; }

        public IDictionary<ushort, TileDesc> Tiles { get; private set; }
        public IDictionary<ushort, Item> Items { get; private set; }
        public IDictionary<ushort, ObjectDesc> ObjectDescs { get; private set; }
        public IDictionary<ushort, PortalDesc> Portals { get; private set; }
        public IDictionary<ushort, PetStruct> TypeToPet { get; private set; }
        public IDictionary<string, PetSkin> IdToPetSkin { get; private set; }

        public void AddObjects(XElement root)
        {
            foreach (XElement elem in root.XPathSelectElements("//Object"))
            {
                if (elem.Element("Class") == null) continue;
                string cls = elem.Element("Class").Value;
                string id = elem.Attribute("id").Value;

                ushort type;
                XAttribute typeAttr = elem.Attribute("type");
                if (typeAttr == null) continue;
                type = (ushort)Utils.FromString(typeAttr.Value);

                if (cls == "PetBehavior" || cls == "PetAbility") continue;

                if (type2id_obj.ContainsKey(type))
                    log.WarnFormat("'{0}' and '{1}' has the same ID of 0x{2:x4}!", id, type2id_obj[type], type);
                if (id2type_obj.ContainsKey(id))
                    log.WarnFormat("0x{0:x4} and 0x{1:x4} has the same name of {2}!", type, id2type_obj[id], id);

                type2id_obj[type] = id;
                id2type_obj[id] = type;
                type2elem_obj[type] = elem;

                switch (cls)
                {
                    case "Equipment":
                    case "Dye":
                        items[type] = new Item((short)type, elem);
                        break;
                    case "Portal":
                    case "GuildHallPortal":
                        try
                        {
                            portals[type] = new PortalDesc(type, elem);
                        }
                        catch
                        {
                            log.Error("Error for portal: " + type + " id: " + id);
                        }
                        break;
                    case "Pet":
                        type2pet[type] = new PetStruct(type, elem);
                        break;
                    case "PetSkin":
                        id2pet_skin[id] = new PetSkin(type, elem);
                        break;
                    case "PetBehavior":
                    case "PetAbility":
                        break;
                    default:
                        objDescs[type] = new ObjectDesc(type, elem);
                        break;
                }
            }
        }

        public void AddGrounds(XElement root)
        {
            foreach (XElement elem in root.XPathSelectElements("//Ground"))
            {
                string id = elem.Attribute("id").Value;

                ushort type;
                XAttribute typeAttr = elem.Attribute("type");
                type = (ushort)Utils.FromString(typeAttr.Value);

                if (type2id_tile.ContainsKey(type))
                    log.WarnFormat("'{0}' and '{1}' has the same ID of 0x{2:x4}!", id, type2id_tile[type], type);
                if (id2type_tile.ContainsKey(id))
                    log.WarnFormat("0x{0:x4} and 0x{1:x4} has the same name of {2}!", type, id2type_tile[id], id);

                type2id_tile[type] = id;
                id2type_tile[id] = type;
                type2elem_tile[type] = elem;

                tiles[type] = new TileDesc(type, elem);
            }
        }

        private void ProcessXml(XElement root)
        {
            AddObjects(root);
            AddGrounds(root);
        }
    }
}