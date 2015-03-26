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
using IProxy.Mod;
using IProxy.Networking;
using IProxy.Networking.ClientPackets;
using IProxy.Networking.ServerPackets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MapRipper
{
    public class Mod : IProxyMod
    {
        public string Name
        {
            get { return "Map Ripper"; }
        }

        public string Description
        {
            get { return "This mod is able to rip maps from prod."; }
        }

        public string Creator
        {
            get { return "ossimc82"; }
        }

        public string RequiredMinimumProxyVersion
        {
            get { return "1.0.0"; }
        }

        public string ModVersion
        {
            get { return "1.0.0"; }
        }

        public string Help
        {
            get { return null; }
        }

        public void Create()
        {
            Singleton<Mod>.SetInstance(this);
        }
    }

    public class MapRipper : PacketHandlerExtentionBase
    {
        private JsonMap map;

        public override bool OnServerPacketReceived(ref Packet packet)
        {
            switch (packet.ID)
            {
                case PacketID.MAPINFO:
                    var mip = Utils.ChangePacketType<MapInfoPacket>(packet);
                    this.map.Init(mip.Width, mip.Height, mip.Name);
                    break;
                case PacketID.UPDATE:
                    var update = Utils.ChangePacketType<UpdatePacket>(packet);
                    foreach (var t in update.Tiles)
                        this.map.Tiles[t.X][t.Y] = t.Tile;

                    foreach (var tileDef in update.NewObjects)
                    {
                        var def = tileDef;
                        var objectClass = Singleton<XmlData>.Instance.ObjectTypeToElement[def.ObjectType].Element("Class").Value;
                        if (objectClass == "Player") continue;

                        if (objectClass != "Enemy" && objectClass != "Character")
                        {
                            def.Stats.Position.X -= 0.5F;
                            def.Stats.Position.Y -= 0.5F;
                        }

                        int _x = (int)def.Stats.Position.X;
                        int _y = (int)def.Stats.Position.Y;
                        Array.Resize(ref map.Entities[_x][_y], map.Entities[_x][_y].Length + 1);
                        ObjectDef[] arr = map.Entities[_x][_y];

                        arr[arr.Length - 1] = def;
                    }
                    break;
            }
            return base.OnServerPacketReceived(ref packet);
        }

        public override bool OnClientPacketReceived(ref Packet packet)
        {
            switch (packet.ID)
            {
                case PacketID.HELLO:
                    this.map = new JsonMap(Singleton<XmlData>.Instance);
                    break;
                case PacketID.PLAYERTEXT:
                    if (Utils.ChangePacketType<PlayerTextPacket>(packet).Text == "/saveMap")
                    {
                        Singleton<Network>.Instance.SendToClient(new TextPacket
                        {
                            BubbleTime = 5,
                            CleanText = "",
                            Name = "",
                            ObjectId = -1,
                            Recipient = "",
                            Stars = -1,
                            Text = "Saving map..."
                        });
                        Singleton<Network>.Instance.SendToClient(new FilePacket
                        {
                            Name = "map_" + this.map.Name + ".jm",
                            Bytes = Encoding.UTF8.GetBytes(this.map.ToJson())
                        });
                        return false;
                    }
                    break;
            }
            return base.OnClientPacketReceived(ref packet);
        }
    }

    public class AssemblyLoader : AssemblyRequestExtentionBase
    {
        public override IEnumerable<Assembly> GetDependencyAssemblies()
        {
            yield return LoadAssemblyFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("MapRipper.Ionic.Zlib.dll"));
            yield return LoadAssemblyFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("MapRipper.Newtonsoft.Json.dll"));
        }
    }
}
