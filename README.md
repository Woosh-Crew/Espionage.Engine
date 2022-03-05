<h1 align="center">
 Espionage.Engine
</h1>

Espionage.Engine is a Unity game base / framework, where you design games off it (duh). 
Its meant to be a similar workflow to Unreal Engine / s&box where you can do a majority of the work for your game straight in code.
This means you should really only be using the editor for asset management, (Maps, Models, etc, setting up Blueprints). This allows
you to do a lotta cool stuff without having to have a spaghetti tree of Unity Scene References.

# General Features Overview
Here is a table that contains an overview of all features, and their current state in development.

| Feature                      |     State      | Description                                                                                                                                            |      API      |
|------------------------------|:--------------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------|:-------------:|
| Reflection ( Library )       |  Ready to Use  | The Library System is just a magical API. We use it everywhere in Espionage.Engine.                                                                    |    Utility    |
| Callbacks                    |  Ready to Use  | Attribute based Event / Callback System.Just fire and forget. `Callback.Run(string)`                                                                   |    Utility    |
| File System ( Files )        |  Ready to Use  | Espionage.Engines System.IO wrapper. Has a string based Pathing System `"config://"` Automatic Deserializers and Serializers, Automatic Saving + more. |    Utility    |
| Auto Converter ( Converter ) |  Ready to Use  | Convert any string to any target object. `var value = Converter.Convert<Vector3>("(23, 42, 59)");`                                                     |    Utility    |
| Component Model              |  Ready to Use  | Anything can have components on it using the `Components<T>` Class.                                                                                    |    Utility    |
| Database Model               |  Ready to Use  | Easily make a wrapper for any type of storage.                                                                                                         |    Utility    |
| Debugging                    |  Ready to Use  | Debugging Library, that Includes Logging, ConVars, ConCmds, and More.                                                                                  |    Utility    |
| Cookies                      | In Development | Allow static properties to retain their value after application shutdown.                                                                              |    Utility    |
| Preferences                  | In Development | Automatically saved Global ConVars, that can have an associated UI element given to it.                                                                |               |
| Maps System                  |  Ready to Use  | Easily load maps at runtime using this simple Library. Just use `Map.Load(string path);` and it'll load the map with the correct deserializer.         |   Resources   |
| Models System                | In Development | Easily load Models at Runtime. Use `Models.Load(string path);` and it'll load it with the correct deserializer, (based off the file extension).        |   Resources   |
| General Asset / Resources    |     Usable     | Easily load any resource at runtime, provided theres a deserializer for it.                                                                            |   Resources   |
| Custom Project Builder       |     Usable     | Export Espionage.Engine games using this Tool. Will automatically transfer all Exported assets and do Espionage.Engine required stuff for performance. |     Tools     |
| Tripod / Camera System       |  Ready to Use  | Easily Create Camera Controller with little overhead. without having to have scene dependencies to your main camera.                                   |  Game / Core  |
| Modding                      | In Development | C# Modding & Easily Creatable Full game overhaul mods.                                                                                                 | Engine / Core |
| Pawn System                  |  Ready to Use  | Pawn System for Clients and AI to easily posses. Allows for super dynamic games while being abstract.                                                  | Engine / Core |
| Inventory System             |     Usable     | Simple Inventory System attached to Actors.                                                                                                            |  Game / Core  |
| Health System                |     Usable     | Simple Health System, IHealable and IDamageable interfaces on a Health Component                                                                       |  Game / Core  |
| Viewmodel System             |  Ready to Use  | Extendable Viewmodel System, Easily create Viewmodel effects using `Viewmodel.IEffect`. Comes prepacked with some such as Sway, Deadzone Sway + more.  |  Game / Core  |
| Input System ( Controls )    |     Usable     | Input Sheet based system, where Camera's and Pawns can control how the input sheet is.                                                                 |  Game / Core  |
| IO System                    |     Usable     | Map IO System similar to Source Engines one. Define Inputs using a Function Attribute and define Outputs using the Output struct.                      |  Game / Core  |
| Runtime Prefabs              |     Usable     | Spawn prefabs using its asset path at runtime. This completely removes the need for DontDestoryOnLoad Prefab Holders.                                  |  Game / Core  |
| Generalised Loader           |     Usable     | Easily Load stuff with a frontend UI to display the progress. Can be used for anything (Multiplayer Joining, Map Loading, etc)                         | Engine / Core |
| Generalised Splash Screen    |     Usable     | Loads this Splash Screen when the game is started. use it for doing any wacky stuff like loading Background Maps for your main menu.                   | Engine / Core |
| Generalised Main Menu        |     Usable     | Easy to use Main Menu System, will load the provided scene and provides you with a base to Easily load back to.                                        | Engine / Core |
| Gamemodes System             |     Usable     | Gamemodes system similar to Unreal Engines.                                                                                                            |  Game / Core  |
| Networking                   |    Planned     | Server Authoritative networking Library. Built from scratch.                                                                                           | Engine / Core |
| Nodes System                 |    Planned     | Easy to use extendable node graph                                                                                                                      | Tools / Nodes |
| AI Behaviour Trees           |    Planned     | Based off the Nodes System, Easily create AI behaviour.                                                                                                |  Game / Core  |
    
# Installing
Simply just install through the package manager. Using the GIT Install type.

# Publicly Released Extensions
Here are a list of our extensions and third party extensions, some are still in early development.

### [Espionage.Engine.Source](https://github.com/Woosh-Crew/Espionage.Engine.Source)
Allows you to natively load Source assets into Espionage.Engine using Espionage.Engine's Resources API.
Just Simply do `Map.Load(string path)` with the path to your BSP for example, and it'll just magically load it.
(Including Textures, Studio Models, Lightmaps, Entities, etc).

<i>This is in super early development!, Would love some help with this.</i>

### [Espionage.Engine.Steam](https://github.com/Woosh-Crew/Espionage.Engine.Steam)
Adds Steamworks functionality to Espionage.Engine using Facepunch.Steamworks. 
Using the Library System, you can easily setup your game using Steam! Just add
the `[Steam(ulong id)]` attribute to your game class.

It comes with a Steam Workshop wrapper, Auto Profile Picture to Texture2D and more.

Espionage.Engine.Steam will automatically setup and initialize Steamworks for you in the background using its
Engine service.

### [Espionage.Engine.Localisation](https://github.com/Woosh-Crew/Espionage.Engine.Localisation)
Espionage.Engines Localisation System. Easily setup new Languages and use them using the Text class, Languages will be automatically
deserialized for you, and will be hot-loaded (Runtime & Editor) if they are changed.

<i>This is in super early development!, Would love some help with this.</i>

### [Espionage.Engine.Sound](https://github.com/Woosh-Crew/Espionage.Engine.Sound)
Espionage.Engine's Sound System. Includes Soundscapes, Steam Audio, Node Based Mixers and more.

<i>This is in super early development!, Would love some help with this.</i>

### [Espionage.Engine.Discord](https://github.com/Woosh-Crew/Espionage.Engine.Discord)
Adds Discord GameSDK functionality to Espionage.Engine.
Using the Library System, you can easily setup your game using Discord! Just add
the `[Discord(ulong id)]` attribute to your game class, and it'll work just like that, no overhead, no setting up Discord callbacks.
That's all done for you behind the scenes (using the Engine service).


# Games & Projects using Espionage.Engine
### Espionage
Espionage.Engine was made for Espionage, since I hated the way Unity worked out of the box.

### Netscape Cybermind
No details yet.

# Help & Support
<div align="center">
 <a href="https://wooshcrew.com">Website</a>&emsp;
 <b>•</b>&emsp;
 <a href="https://twitter.com/JakeWoosh">YouTube</a>&emsp;
 <b>•</b>&emsp;
 <a href="https://wooshcrew.com/discord">Discord</a>&emsp;
</div>
