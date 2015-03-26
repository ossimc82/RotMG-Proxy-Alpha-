﻿#region

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

namespace common.data
{
    public class XmlData : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (XmlData));

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

        private string[] addXml;
        private int prevUpdateCount = -1;
        private int updateCount;

        public XmlData(string path = "data")
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

            //assign = new AutoAssign(this);

            string basePath = Path.Combine(AssemblyDirectory, path);
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

        public string[] AdditionXml
        {
            get
            {
                UpdateXml();
                return addXml;
            }
        }

        public void Dispose()
        {
            //assign.Dispose();
        }

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
                //    type = assign.Assign(id, elem);
                type = (ushort)ServerUtils.FromString(typeAttr.Value);

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
                            Console.WriteLine("Error for portal: " + type + " id: " + id);
                            /*3392,1792,1795,1796,1805,1806,1810,1825 -- no location, assume nexus?* 
        *  Tomb Portal of Cowardice,  Dungeon Portal,  Portal of Cowardice,  Realm Portal,  Glowing Portal of Cowardice,  Glowing Realm Portal,  Nexus Portal,  Locked Wine Cellar Portal*/
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

                XAttribute extAttr = elem.Attribute("ext");
                bool ext;
                if (extAttr != null && bool.TryParse(extAttr.Value, out ext) && ext)
                {
                    if (elem.Attribute("type") == null)
                        elem.Add(new XAttribute("type", type));
                    updateCount++;
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
                type = (ushort)ServerUtils.FromString(typeAttr.Value);

                if (type2id_tile.ContainsKey(type))
                    log.WarnFormat("'{0}' and '{1}' has the same ID of 0x{2:x4}!", id, type2id_tile[type], type);
                if (id2type_tile.ContainsKey(id))
                    log.WarnFormat("0x{0:x4} and 0x{1:x4} has the same name of {2}!", type, id2type_tile[id], id);

                type2id_tile[type] = id;
                id2type_tile[id] = type;
                type2elem_tile[type] = elem;

                tiles[type] = new TileDesc(type, elem);

                XAttribute extAttr = elem.Attribute("ext");
                bool ext;
                if (extAttr != null && bool.TryParse(extAttr.Value, out ext) && ext)
                {
                    updateCount++;
                }
            }
        }

        private void ProcessXml(XElement root)
        {
            AddObjects(root);
            AddGrounds(root);
        }

        private void UpdateXml()
        {
            if (prevUpdateCount != updateCount)
            {
                prevUpdateCount = updateCount;
            }
        }

        private class AutoAssign : SimpleSettings
        {
            private XmlData dat;
            private int nextFullId;
            private int nextSignedId;

            internal AutoAssign(XmlData dat)
                : base("autoId")
            {
                this.dat = dat;
                nextSignedId = GetValue<int>("nextSigned", "24576"); //0x6000
                nextFullId = GetValue<int>("nextFull", "32768"); //0x8000
            }

            public int Assign(string id, XElement elem)
            {
                int type = GetValue<int>(id, "0");
                if (type == 0)
                {
                    XElement cls = elem.Element("Class");
                    bool isFull = cls.Value == "Dye" ||
                                  (cls.Value == "Equipment" && !elem.Elements("Projectile").Any());
                    if (isFull)
                    {
                        type = nextFullId++;
                        SetValue("nextFull", nextFullId.ToString());
                    }
                    else
                    {
                        type = nextSignedId++;
                        SetValue("nextSigned", nextSignedId.ToString());
                    }
                    SetValue(id, type.ToString());
                    log.InfoFormat("Auto assigned '{0}' to 0x{1:x4}", id, type);
                }
                return type;
            }
        }
    }
}