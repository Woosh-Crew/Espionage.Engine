<h1 align="center">
 Espionage.Engine
</h1>

Espionage.Engine is a Unity game base, where you design games off it (duh). Its meant to be a similar workflow to Unreal Engine or s&box where you can do a majority of the work for your game straight in code.
 
## Current Features
It comes with a handful of nice features and major workflow changes.

### > Callback System
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

### > Console System / Debugging Library
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

## Planned Features
These are planned features that are going to be implemented in the future

### > Generic Node Graph
This system will provide a generic node graph that can be applied to anything. Examples include, visual scripting, node based audio engine, node based animation controller, UI node network, etc. 
 
### > Networking
Networking is a fork of Tom Weiland's Riptide, with major improvements to it. Such as:
- Networked objects being indicted by using an interface, where it implements a network identity object.

### > Assets
A majority of assets should be loaded at runtime using AssetBundles (Such as Scenes, Possibly textures and sounds) this provides many benefits such as everything not being scene dependent.

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
