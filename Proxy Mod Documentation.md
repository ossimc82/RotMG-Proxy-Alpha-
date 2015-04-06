# Mod Documentation
----------------------------------------

## Getting Started
----------------------------------------
**Please Note: This documentation is written for Visual Studio 2013**

First steps:
1) Create a new C# "Class Library" Project.

2) Rightclick your project in the Solution Explorer and select "Add Reference".

3) Go to the "Browse" tab and find and add "IProxy.dll".

4) Rename the default class that was created from "Class1" to something more meaningful (I sugest "Mod") and begin editing the class file.

5) Add references the following namespaces in your class file:
- `using IProxy.Mod;`

6) Make your class public and implement "IProxyMod". eg "public class Mod : IProxyMod".

## `IProxy.Mod::IProxyMod` Interface
----------------------------------------
The `IProxy.Mod::IProxyMod` interface defines the "EntryPoint" from your mod.

> ###Syntax: 
> `public interface IProxyMod`

Things to note are:
- Only one instance of your mod will be created.
- Since IProxyMod is an interface, you must implement all functions and properties.
- The core itself is thread safe, but other mods might create other threads, so make it thread safe.
- You can give your class a custom constructor as long as it doesnt take any parameters. Otherwise the mod will not be loaded

IProxyMod constists of the following properties and methods:
- `string Name { get; }` This is the name of your mod.
- `string Description { get; }` This is a description of what your mod does.
- `string Creator { get; }` This is your name, the creator of the mod.
- `string RequiredMinimumProxyVersion { get; }` This needs to return the minimum required version of the prox, eg "1.0.0"
- `string ModVersion { get; }` This should return the current version of your mod, eg "1.0.0"
- `string Help { get; }` This should return some additional help about your mod.
- `void Create();` This method will be called when your mod is accepted by the proxy. Your creation logic will go into this method

**Please note: Versions need 3 numbers to work:**
***"1.4.9"***
> - 1 - Major Version
> - 4 - Minor Version
> - 9 - Build Version 

## `IProxy::Singleton<T>` Class
----------------------------------------
The `IProxy::Singleton<T>` class is a very powerfull class. It will allow you to save instances of a class and access with static calls.

> ###Syntax: 
> `public sealed class Singleton<T> where T : class, new()`

- `static void Seal()` This will seal the singleton, so the instance can not be recreated.
- `static T SetInstance(T instance, bool seal=true)` This will set a new instance to your singleton as long as it is not sealed
- `static bool IsSealed { get; }` Returns if the singleton is sealed, true if sealed.
- `static T Instance { get; }` Returns the instance of the class, if the instance is null it will create a new instance as long as the singleton is not sealed.

> ###Usage:
> ```C#
> public class MySingleton
> {
>     public int MyInt;
>      
>     public MySingleton()
>     {
>         MyInt = 10;
>     }
>
>	  public MySingleton(int value)
>     {
>         MyInt = value;
>     }
> }
>
> public class Access
> {
>     public static void WriteMyInt()
>     {
>         Console.WriteLine(Singleton<MySingleton>.Instance.MyInt);
>         Singleton<MySingleton>.SetInstance(new MySingleton(20));
>         Console.WriteLine(Singleton<MySingleton>.Instance.MyInt);
>     }
> }
> //If Access.WriteMyInt() is called
> //Output: 10
> //Output: 20
> ```

**Please note: You can only use classes as singleton if they provide a parameterless constuctor**

## `IProxy::Network` Class and `IProxy::Network.NetworkClient` Struct
----------------------------------------
### `IProxy::Network`
***Sealed Singleton member***

The `IProxy.Network` class will provide some basic networking methods.

> ###Syntax: 
> `public class Network`

- `NetworkClient Client { get; }` Returns a NetworkClient instance with some basic methods for the chat.
- `void SendToServer(Packet packet)` Sends a packet to the server.
- `void SendToClient(Packet packet)` Sends a packet to the client.
- `void After(int milliseconds, Action action)` Performs an action after a certain time


- `event PacketDelegate OnSendToServer` Fires when "SendToServer" is called.
- `event PacketDelegate OnSendToClient` Fires when "SendToClient" is called.
***Its not recommended to attach any listeners to this 2 events***

### `IProxy::Network.NetworkClient`
***Member of `IProxy::Network`***
> ###Syntax: 
> `public struct NetworkClient`

- `void SendInfo(string text)` Sends a normal yellow text to the client.
- `void SendHelp(string text)` Sends text with the orange "help" color to the client.
- `void SendError(string text)` Sends a red error text to the client.
- `void SendGuild(string text)` Sends green guild text to the client.
- `void SendEnemy(string enemy, string text)` Sends an enemy text to the client.
- `void SendAnnouncement(string text)` Sends an announcement to the client.
- `void SendTell(string from, string text, string to="", int objId=-1, int stars=-1)` Sends blue tell text to the client.

## `IProxy.common.data::XmlData` Class
----------------------------------------
***Sealed Singleton member***
The `IProxy.common.data::XmlData` represents a collection of the current xml data from the game.

> ###Syntax: 
> `public class XmlData`

- `IDictionary<ushort, XElement> ObjectTypeToElement { get; }` ID-XElement combination
- `IDictionary<ushort, string> ObjectTypeToId { get; }` ID-Name combination
- `IDictionary<string, ushort> IdToObjectType { get; }` Name-ID combination
- `IDictionary<ushort, XElement> TileTypeToElement { get; }` TileId-XElement combination
- `IDictionary<ushort, string> TileTypeToId { get; }` TileId-TileName combination
- `IDictionary<string, ushort> IdToTileType { get; }` TileName-TileId combination
- `IDictionary<ushort, TileDesc> Tiles { get; }` TileId-TileDesc combination
- `IDictionary<ushort, Item> Items { get; }` ItemId-ItemDesc combination
- `IDictionary<ushort, ObjectDesc> ObjectDescs { get; }` ObjectId-ObjectDesc combination
- `IDictionary<ushort, PortalDesc> Portals { get; }` PortalId-PortalDesc combination
- `IDictionary<ushort, PetStruct> TypeToPet { get; }` PetId-PetDesc combination
- `IDictionary<string, PetSkin> IdToPetSkin { get; }` PetSkinName-PetSkinDesc combination

## `IProxy.Mod::ISettingsProvider` Interface
----------------------------------------
The `IProxy.Mod::ISettingsProvider` interface will tell the proxy that your mod stores specific settings.

> ###Syntax
> `public interface ISettingsProvider : IDisposable`

- `ISettingsProvider Register(string modId);` This method will be called when your mod will initialize.
- `T GetValue<T>(string key, string def=null);` This method needs to return a T for your settings key.
- `void SetValue(string key, string value);` This method needs to set the value as a string for the certain key.
- `void Save();` This method needs to save your settings. This method will always be called if you close the proxy.
- `IEnumerable<KeyValuePair<string, string>> GetValues();` This method needs to return all your settings.

**Note: If you want easy and quick handling of your settings use the pre-defined class `IProxy.common::SimpleSettings`
The usage is shown below**

> ###Usage
> ```C#
> using IProxy.common;
> 
> public class Settings : ISettingsProvider
> {
>    private SimpleSettings settings;
>
>    public ISettingsProvider Register(string modId)
>    {
>        //Initialize a new Instance of SimpleSettings
>        settings = new SimpleSettings(modId);
>        
>        //Register the setting values
>        GetValue<float>("myFloatSetting", "100.0");
>        GetValue<string>("myStringSetting", "u r cool");
>        
>        //Return this class and set it as a sealed singleton to access it in the mod
>        return Singleton<Settings>.SetInstance(this);
>    }
>
>    public T GetValue<T>(string key, string def=null)
>    {
>        return settings.GetValue<T>(key, def);
>    }
>
>    public void SetValue(string key, string value)
>    {
>        settings.SetValue(key, value);
>    }
>
>    public void Save()
>    {
>        settings.Save();
>    }
>
>    public void Dispose()
>    {
>        settings.Dispose();
>    }
>
>    public IEnumerable<KeyValuePair<string, string>> GetValues()
>    {
>        return settings.GetValues();
>    }
>}
> ```

## `IProxy.Mod::ProxyExtentionBase` Class
----------------------------------------
The `IProxy.Mod::ProxyExtentionBase` class is a basic class in the proxy for some Extention bases.

> ###Syntax
> `public class ProxyExtentionBase : IDisposable`

- `virtual bool DisposeAfterDisconnect()` Return true if you want to call dispose when you disconnect ingame.
- `virtual void OnDisconnect()` Will be called if you disconnect ingame.
- `virtual void Dispose()` Will be called if you disconnect ingame and "DisposeAfterDisconnect" returns true or when you close the proxy.

**Note: The proxy will ignore it if you inherit directly from this class**

## `IProxy.Mod::PacketHandlerExtentionBase` Class
----------------------------------------
The `IProxy.Mod::PacketHandlerExtentionBase` class is a basic packet handler for your mod.

> ###Syntax
> `public class PacketHandlerExtentionBase : ProxyExtentionBase`

- `virtual bool OnServerPacketReceived(ref Packet packet)` Return false if you dont want to send the packet to the client.
- `virtual bool OnClientPacketReceived(ref Packet packet)` Return false if you dont want to send the packet to the server.

> ###Usage
> ```C#
> public class PacketHandler : PacketHandlerExtentionBase
> {
>     public override bool OnClientPacketReceived(ref Packet packet)
>     {
>         if (packet.ID == PacketID.INVSWAP) return false; //Dont send InvSwap packets to the server
>         return true;
>     }
> 
>     public override bool OnServerPacketReceived(ref Packet packet)
>     {
>         if (packet.ID == PacketID.SHOW_EFFECT) return false; //Dont send ShowEffect packets to the client
>         return true;
>     }
> }
> ```

## `IProxy.Mod::AdvancedPacketHandlerExtentionBase` Class
----------------------------------------
The `IProxy.Mod::AdvancedPacketHandlerExtentionBase` class is a more advanced packet handler for your mod.

> ###Syntax
> `public abstract class AdvancedPacketHandlerExtentionBase : PacketHandlerExtentionBase`

- `delegate bool PacketReceive<T>(ref T packet) where T : Packet;` The delegate for a defined packet.
- `delegate bool GeneralPacketReceive<T>(ref Packet packet) where T : Packet;` The delegate for general packets.
- `abstract void HookPackets()` This method will be called to hook your packets.
- `void ApplyPacketHook<T>(PacketReceive<T> callback) where T : Packet` This will hook a packet type to the defined callback. T is PacketType
- `void ApplyGeneralPacketHook<T>(GeneralPacketReceive<T> callback) where T : Packet` This will hook a packet type to a general callback. T is PacketType

> ###Usage
> ```C#
> public class MyPacketHandler : AdvancedPacketHandlerExtentionBase
> {
>     protected override void HookPackets()
>     {
>         ApplyPacketHook<MapInfoPacket>(OnMapInfoPacket);
>         ApplyGeneralPacketHook<CreateSuccessPacket>(OnGeneralPacketReceived);
>         ApplyGeneralPacketHook<ShowEffectPacket>(OnGeneralPacketReceived);
>         ApplyGeneralPacketHook<HelloPacket>(OnGeneralPacketReceived);
>     }
> 
>     private bool OnMapInfoPacket(ref MapInfoPacket packet)
>     {
>         Console.WriteLine("Current map name is: " + packet.Name);
>         return true;
>     }
>    
>	  private bool OnGeneralPacketReceived(ref Packet packet)
>	  {
>	      Console.WriteLine("Also received: " + packet.ID);
>	      
>	      if (packet.ID == PacketID.SHOW_EFFECT)
>	          return false; //Dont send ShowEffect packets.
>	      return true;
>	  }
> }
> ```

## `IProxy.Mod::ICommandManager` Interface
----------------------------------------
The `IProxy.Mod::ICommandManager` interface will tell the proxy that your mod is using custom ingame commands.

> ###Syntax
> `public interface ICommandManager`

- `IEnumerable<string> RegisterCommands()` This Method will be called to register your commands, the `/` at the beginning will be ignored if you set it. Commands are 1-Word only (No Whitespace).
- `bool OnCommandGet(string command, string[] args)` This method is fired when you type a command ingame. The command parameter specifies the command string and args are all words after the actual command seperated by whitespaces. Returns if the PlayerTextPacket that is bound to the command should be send to the server.

**Note: This interface can be attached to the `IProxy.Mod::PacketHandlerExtentionBase` class**

> ###Usage
> ```C#
> public class MyCommandHandler : ICommandManager
> {
>     public IEnumerable<string> RegisterCommands()
>     {
>         yield return "writeFirstArg";
>         yield return "writeHi";
>     }
>
>     public bool OnCommandGet(string command, string[] args)
>     {
>         switch(command)
>         {
>             case "writeFirstArg":
>                 Singleton<Network>.Instance.Client.SendInfo(args[0]);
>                 return false; //Dont send "/writeFirstArg" to the server
>                 
>             case "writeHi":
>                 Console.WriteLine("Hi");
>                 return true; //Send "/writeHi" to the server
>         }
>         return true;
>     }
> }
> ```


## `IProxy.Mod::AdvancedCommandManager` Class
----------------------------------------
The `IProxy.Mod::AdvancedCommandManager` class is a more advanced command handler for your mod.

> ###Syntax
> `public abstract class AdvancedCommandManager : ICommandManager`

- `delegate bool Command(string[] args)` The command callback delegate.
- `abstract void HookCommands();` This method will be called to hook your commands to a callback.
- `void ApplyCommandHook(string command, Command callback)` Applies a callback to the specified command.
- *Members implemented by ICommandManager will be handled by the class itself*

> ###Usage
> ```C#
> public class CommandManager : AdvancedCommandManager
> {
>     public override void HookCommands()
>     {
>         ApplyCommandHook("myCommand", OnMyCommand);
>     }
> 
>     private bool OnMyCommand(string[] args)
>     {
>         Console.WriteLine("myCommand get with {0} args", args.Length);
>         return false; //Dont send the command to the server
>     }
> }
> ```


## `IProxy::Server` Class
----------------------------------------
***Sealed Singleton member***
The `IProxy::Server` class is used by the proxy to connect to the target server

> ###Syntax
> `public class Server`

- `string DefaultHost { get; set; }` Gets or sets the Default Server IP
- `string CurrentHost { get; set; }` Gets or sets the Current Server IP
- `int DefaultPort { get; set; }` Gets or sets the Default Server Port
- `int CurrentPort { get; set; }` Gets or sets the Current Server Port

> ###Usage
> ```C#
>     public void SetNextServer(string host, int port)
>     {
>         Singleton<Server>.Instance.DefaultHost = host;
>         Singleton<Server>.Instance.DefaultPort = port;
>     }
> ```


## `IProxy::PacketID` Enumeration
----------------------------------------
The `IProxy::PacketID` enum stores all packet ids in it. Its the reccomended way to determine what packet is what type instead of using the raw packet Ids. This way, mods still work when IDs change.

> ###Syntax
> `public enum PacketID : byte`

Values of the enum:

        FAILURE = 0, //slotid: 1
		CREATE_SUCCESS = 27, //slotid: 2
		CREATE = 17, //slotid: 3
		PLAYERSHOOT = 58, //slotid: 4
		MOVE = 7, //slotid: 5
		PLAYERTEXT = 6, //slotid: 6
		TEXT = 55, //slotid: 7
		SHOOT2 = 76, //slotid: 8
		DAMAGE = 44, //slotid: 9
		UPDATE = 23, //slotid: 10
		UPDATEACK = 92, //slotid: 11
		NOTIFICATION = 42, //slotid: 12
		NEW_TICK = 88, //slotid: 13
		INVSWAP = 13, //slotid: 14
		USEITEM = 1, //slotid: 15
		SHOW_EFFECT = 53, //slotid: 16
		HELLO = 84, //slotid: 17
		GOTO = 41, //slotid: 18
		INVDROP = 34, //slotid: 19
		INVRESULT = 60, //slotid: 20
		RECONNECT = 59, //slotid: 21
		PING = 18, //slotid: 22
		PONG = 97, //slotid: 23
		MAPINFO = 78, //slotid: 24
		LOAD = 75, //slotid: 25
		PIC = 28, //slotid: 26
		SETCONDITION = 79, //slotid: 27
		TELEPORT = 81, //slotid: 28
		USEPORTAL = 85, //slotid: 29
		DEATH = 35, //slotid: 30
		BUY = 4, //slotid: 31
		BUYRESULT = 16, //slotid: 32
		AOE = 12, //slotid: 33
		GROUNDDAMAGE = 91, //slotid: 34
		PLAYERHIT = 31, //slotid: 35
		ENEMYHIT = 37, //slotid: 36
		AOEACK = 66, //slotid: 37
		SHOOTACK = 48, //slotid: 38
		OTHERHIT = 74, //slotid: 39
		SQUAREHIT = 67, //slotid: 40
		GOTOACK = 50, //slotid: 41
		EDITACCOUNTLIST = 96, //slotid: 42
		ACCOUNTLIST = 65, //slotid: 43
		QUESTOBJID = 57, //slotid: 44
		CHOOSENAME = 3, //slotid: 45
		NAMERESULT = 77, //slotid: 46
		CREATEGUILD = 33, //slotid: 47
		CREATEGUILDRESULT = 63, //slotid: 48
		GUILDREMOVE = 52, //slotid: 49
		GUILDINVITE = 80, //slotid: 50
		ALLYSHOOT = 19, //slotid: 51
		SHOOT = 69, //slotid: 52
		REQUESTTRADE = 11, //slotid: 53
		TRADEREQUESTED = 10, //slotid: 54
		TRADESTART = 47, //slotid: 55
		CHANGETRADE = 95, //slotid: 56
		TRADECHANGED = 45, //slotid: 57
		ACCEPTTRADE = 49, //slotid: 58
		CANCELTRADE = 56, //slotid: 59
		TRADEDONE = 64, //slotid: 60
		TRADEACCEPTED = 61, //slotid: 61
		CLIENTSTAT = 20, //slotid: 62
		CHECKCREDITS = 39, //slotid: 63
		ESCAPE = 22, //slotid: 64
		FILE = 38, //slotid: 65
		INVITEDTOGUILD = 51, //slotid: 66
		JOINGUILD = 15, //slotid: 67
		CHANGEGUILDRANK = 26, //slotid: 68
		PLAYSOUND = 5, //slotid: 69
		GLOBAL_NOTIFICATION = 68, //slotid: 70
		RESKIN = 14, //slotid: 71
		PETYARDCOMMAND = 40, //slotid: 72
		PETCOMMAND = 87, //slotid: 73
		UPDATEPET = 8, //slotid: 74
		NEWABILITYUNLOCKED = 46, //slotid: 75
		UPGRADEPETYARDRESULT = 36, //slotid: 76
		EVOLVEPET = 98, //slotid: 77
		REMOVEPET = 83, //slotid: 78
		HATCHEGG = 82, //slotid: 79
		ENTER_ARENA = 30, //slotid: 80
		ARENANEXTWAVE = 25, //slotid: 81
		ARENADEATH = 86, //slotid: 82
		LEAVEARENA = 94, //slotid: 83
		VERIFYEMAILDIALOG = 9, //slotid: 84
		RESKIN2 = 24, //slotid: 85
		PASSWORDPROMPT = 21, //slotid: 86
		VIEWQUESTS = 93, //slotid: 87
		TINKERQUEST = 62, //slotid: 88
		QUESTFETCHRESPONSE = 90, //slotid: 89
		QUESTREDEEMRESPONSE = 89 //slotid: 90

## `IProxy.Mod::AssemblyRequestExtentionBase` Class
----------------------------------------
The `IProxy.Mod::AssemblyRequestExtentionBase` class tells the proxy that your mod is using external assemblies.

> ###Syntax
> `public abstract class AssemblyRequestExtentionBase : ProxyExtentionBase`

- `abstract IEnumerable<Assembly> GetDependencyAssemblies()` Returns the assemblies that your mod will use.
- `Assembly LoadAssemblyFromStream(Stream assemblyStream)` Returns an assembly loaded in runtime.

**Note: You need to embed the .dll file into your project and you need to specify the full name of the assembly.
For more information on how to use the `GetManifestResourceStream` method, please visit: http://stackoverflow.com/questions/3314140/how-to-read-embedded-resource-text-file**

> ###Usage
> ```C#
> public class AssemblyLoader : AssemblyRequestExtentionBase
> {
>     public override IEnumerable<Assembly> GetDependencyAssemblies()
>     {
>         yield return LoadAssemblyFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("MyProjectNameSpace.MyAssemblyToLoad.dll"));
>     }
> }
> ```


## To be continued...