<h1 align="center">
 Espionage.Engine
</h1>

Espionage.Engine is a Unity game base, where you design games off it (duh). Its meant to be a similar workflow to Unreal Engine or s&box where you can do a majority of the work for your game straight in code.
 
# Current Features
It comes with a handful of nice features and major workflow changes that improve development time by a ton.

## > Assets System
Espionage.Engine provides a simple asset system that uses AssetBundles (or anything if you code your own assets) for loading and unloading assets. This API can be found in the "Resource" module.
If you compile an asset inside your games unity project, itll automatically be added to your built project if you use the Espionage.Engine project builder (which you should be using by default). 

#### Features
* API, The API provides an incredibly easy way for references assets that need to be loaded and unloaded
* Comes with pre-made asset types
  * UI
  * Decal
  * Surface
  * Interface
  * Sound

## > Map Management
The map system uses the asset system for loading and unloading maps at runtime through AssetBundles. this saves the need of having to have all your maps precompiled with the game. 
This also allows for custom content to be easily created through custom maps and practically allows your games community to make the game for you. Extending your games shelf life.

You can find this in all you need in the Map class.

If you also have the Espionage.Engine.Steam module, it'll automatically track playtime for that map for your games workshop page. 

## > Reflection System / Library
The library system adds support for adding meta data to classes and properties. You can do a tone of cool stuff with this, while maintaining clean code.
the library system is used almost everywhere in Espionage.Engine, as it provides an incredibly performant way for handing reflection, (Around 25ms cache)

To add an item for Library caching, simply add the Library attribute or add the ILibrary interface

*Cache through Interface*
``` csharp
public class Entity : Object, ILibrary 
{ 
    // ILibrary requires us to add the ClassInfo property
    Library ClassInfo { get; set; }

    public Entity()
    {
        // When can then get it using the Database property on the Library class
        ClassInfo = Library.Database[GetType()];
    }
}
``` 

*Cache through Attribute*
``` csharp
// If you have a static class you want to cache through the library system
// You need to use the attribute, since you can't add interfaces to static classes

[Library]
public static class Utility  { }
``` 

#### Features
* Spawning objects with a string identifier
* Adding per-class meta.

## > Callback System
Easy attribute based event system that I'm calling the Callback system. Just assign a method the "Callback" attribute and it'll do the rest behind the scenes.
``` csharp
[Callback( "callback.frame" )]
private void Frame() { }
``` 
Or you can predefined your own attributes to auto assign the callback property.
``` csharp
[Callback.Frame]
private void Frame() { }
``` 
As it says theses callbacks will invoke that method every frame. This will work on static or instanced objects. If you want it to work for instance objects you have to register it in the callback database, so it knows to invoke it.

Then you can invoke it using this expression:
``` csharp
Callback.Run( "callback.name" ); 
``` 
the string being the callback

## > Console System / Debugging Library
The Debugging Library contains a wide variety of helpful methods and tools to help you debug your game. All of which can be found under the Debugging static class.

#### Logging System
Easily extendable logging system using extension methods or your own logging provider. To log something simply do:
``` csharp
Debugging.Log.Info( "Wasssup" );
Debugging.Log.Error( "oh no! something went wrong!" )
``` 

#### Stopwatch Scope
Simple to use stopwatch scope, will record the time for anything in the scope and will log a message for how long it took.

``` csharp
using ( Debugging.Stopwatch( "blah blah" ) )
{
   // Some long task.
}

// When finished
// LOG: blah blah | 40ms 
```

#### Console / Commands
Attribute based and easily extendable

using the `Debugging.Cmd` attribute, you can easily define console commands to be invoked
``` csharp
[Debugging.Cmd( "mat.override_viewport", Help = "Changes the viewport shader to a debug shader, for debugging visuals" )]
private static void ChangeViewportDebug( int shaderId ) 
{
   // Blah blah, change viewport replace shader
}
``` 
or alternatively you can use the `Debugging.Var` attribute on a property, so you can change it or use it as a readonly value
```csharp
[Debugging.Var( "sv.cheats", Help = "Enable cheats on the server", IsReadOnly = false )
private static bool Cheats { get; set; } = false;
```

# Planned Features
These are planned features that are going to be implemented in the future

## > Generic Node Graph
This system will provide a generic node graph that can be applied to anything. Examples include, visual scripting, node based audio engine, node based animation controller, UI node network, etc. 
 
### > Networking
Networking is a fork of Tom Weiland's Riptide, with major improvements to it. Such as:
- Networked objects being indicted by using an interface, where it implements a network identity object.

# Publicly Released Extensions
Here are a list of our extensions and third party extensions.
### [Espionage.Engine.Steam](https://github.com/Woosh-Crew/Espionage.Engine.Steam)
Adds Steamworks functionality to Espionage.Engine using Facepunch.Steamworks.

### [Espionage.Engine.Discord](https://github.com/Woosh-Crew/Espionage.Engine.Discord)
Adds Discord GameSDK functionality to Espionage.Engine.


# Games & Projects using Espionage.Engine
### Espionage
Espionage.Engine was made for Espionage, since I hated the way Unity worked.

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
