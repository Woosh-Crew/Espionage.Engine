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
The library system adds support for adding meta data to classes and properties using a component based model. You can do a tone of cool stuff with this, while maintaining clean code.
the library system is used almost everywhere in Espionage.Engine, as it provides an incredibly performant way for handing reflection, (Around 25ms cache).

To add an item for Library caching, simply add the Library attribute or add the ILibrary interface

*Cache through Interface*
```c#
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
``` c#
// If you have a static class you want to cache through the library system
// You need to use the attribute, since you can't add interfaces to static classes

[Library]
public static class Utility  { }
``` 

### General Features
* Spawning objects with a string identifier
* Adding per-class meta.

### How do we use the Library system in Espionage.Engine? 
We use it almost everywhere, like I stated above. But here are some cool uses of it

#### Map.cs
```c#
[Title( "Map" ), Group( "Maps" ), File( Extension = "map" ), Manager( nameof( Cache ), Order = 600, Layer = Layer.Editor | Layer.Runtime )]
public sealed partial class Map : IResource, IDisposable, IAsset, ILibrary
{
    // Code
}
```
As you can see we give this file a lot of meta data, such as defining File Extensions, Groups etc, and then we later use that on export for defining those characteristics to a file, such as the group being the export path, and the Extension being the file extension

#### Element.cs
In element.cs we use ILibrary for something cooler. we use it for getting the style sheets of the child classes for automatically assigning them.

```c#
public Element()
{
    ClassInfo = Library.Database.Get( GetType() );
    Callback.Register( this );
    foreach ( var item in ClassInfo.Components.GetAll<StyleSheetAttribute>() )
    {
        styleSheets.Add( item.Style );
    }
}
```

As you can see here we assign the style sheet on construct.
Now in one of our UI element classes **HeaderBar.cs** we inherit from Element which will automatically assign the stylesheet based of that StyleSheet Library component.

```c#
[StyleSheet( GUID = "4f913390e11109d438024966ae758619" )]
public class HeaderBar : Element { }
```

#### Engine.cs
In the engine class we use the library system for getting the game class so we can spawn it.

```c#
// Method inside Engine.cs
private static void Initialize()
{
    // Here we get the first result where it isn't abstract.
    var target = Library.Database.GetAll<Game>().FirstOrDefault( e => !e.Class.IsAbstract );
    
    // If the library system couldn't find a result just exit
    if ( target is null )
    {
        Debugging.Log.Warning( "Game couldn't be found." );
        Callback.Run( "game.not_found" );
        return;
    }
    
    using ( Debugging.Stopwatch( "Engine / Game Ready" ) )
    {
        // Create the game class from the library we accessed before
        Game = Library.Database.Create<Game>( target.Class );
 
        // Ready Up Project
        Callback.Run( "game.ready" );
        Game.OnReady();
    }
```

#### EditorTool.cs
For tools in Espionage.Engine we use the library system for defining them. EditorTool.cs will automatically assign the title, stylesheet, icon and tooltip depending on what meta data you have
on the child class.

Here we set the Title and Tooltip
```c#
// Method inside EditorTool for caching the library, and assigning the values
protected virtual void OnEnable()
{
    ClassInfo = Library.Database[GetType()];

    titleContent = new GUIContent( ClassInfo.Title, ClassInfo.Help );

    if ( ClassInfo.Components.TryGet<IconAttribute>( out var icon ) )
    {
        titleContent.image = icon.Icon;
    }
}
```

Here where we create the GUI we assign the StyleSheet
```c#
private void CreateGUI()
{
    // See if we have the StyleSheet component, if we do add it
    if ( ClassInfo.Components.TryGet<StyleSheetAttribute>( out var style ) )
    {
        rootVisualElement.styleSheets.Add( style.Style );
    }
    
    OnCreateGUI();
}
```

And here we make a generic menu that allows us to launch it another tool when we click on one of the menu items

```c#
// Create Tools Menu
var toolsMenu = new GenericMenu();
_menuBar.Add( "Tools", toolsMenu );

foreach ( var item in Library.Database.GetAll<EditorTool>() )
{
    if ( !string.Equals( item.Group, "hidden", StringComparison.CurrentCultureIgnoreCase ) )
    {
        toolsMenu.AddItem( new GUIContent( string.IsNullOrEmpty( item.Group ) ? "" : $"{item.Group}/" + item.Title ), false, () => GetWindow( item.Class ) );
    }
}
```

Now in one of our tools, all the hard work and boilerplate code is not needed anymore.
```c#
[Title( "Project Builder" ), Group( "Compiler" )]
[Icon( EditorIcons.Code ), HelpURL( "https://github.com/Woosh-Crew/Espionage.Engine/wiki" )]
[StyleSheet( GUID = "286338582a0f405dad4fcb85ab99dcc7" )]
public class ProjectBuilder : EditorTool { }
```

You can do other crazy cool stuff I haven't gone through here, just look through Espionage.Engines code base to see how extensively I've used the Library System.

## > Callback System
Easy attribute based event system that I'm calling the Callback system. Just assign a method the "Callback" attribute and it'll do the rest behind the scenes.
```c#
[Callback( "callback.frame" )]
private void Frame() { }
``` 
Or you can predefined your own attributes to auto assign the callback property.
```c#
[Callback.Frame]
private void Frame() { }
``` 
As it says theses callbacks will invoke that method every frame. This will work on static or instanced objects. If you want it to work for instance objects you have to register it in the callback database, so it knows to invoke it.

Then you can invoke it using this expression:
```c#
Callback.Run( "callback.name" ); 
``` 
the string being the callback

## > Console System / Debugging Library
The Debugging Library contains a wide variety of helpful methods and tools to help you debug your game. All of which can be found under the Debugging static class.

#### Logging System
Easily extendable logging system using extension methods or your own logging provider. To log something simply do:
```c#
Debugging.Log.Info( "Wasssup" );
Debugging.Log.Error( "oh no! something went wrong!" )
``` 

#### Stopwatch Scope
Simple to use stopwatch scope, will record the time for anything in the scope and will log a message for how long it took.

```c#
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
```c#
[Debugging.Cmd( "mat.override_viewport", Help = "Changes the viewport shader to a debug shader, for debugging visuals" )]
private static void ChangeViewportDebug( int shaderId ) 
{
   // Blah blah, change viewport replace shader
}
``` 
or alternatively you can use the `Debugging.Var` attribute on a property, so you can change it or use it as a readonly value
```c#
[Debugging.Var( "sv.cheats", Help = "Enable cheats on the server", IsReadOnly = false )
private static bool Cheats { get; set; } = false;
```

## > Localisation System
This is currently in Alpha, and may not work at times. Readme will be updated once finished

## > Camera System
This is also currently in Alpha, and may not work at times. Readme will be updated once finished

# Planned Features
These are planned features that are going to be implemented in the future

## > Generic Node Graph
This system will provide a generic node graph that can be applied to anything. Examples include, visual scripting, node based audio engine, node based animation controller, UI node network, etc. 

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
