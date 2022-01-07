# Espionage.Engine
Espionage.Engine is a Unity game base, where you design games off it (duh)
 
## Features
It comes with a handfull of nice features and major workflow changes.
 
### > Networking
Networking is based of mirror, with major improvements to it.
Such as:
- Networked objects being indicted by using an interface
- Not using / being inherited from MonoBehaviour

### > Entity System
An entity system similar to source. So its super easy to make custom maps etc, without having to update a map maker code base everytime you impliment a new feature. It uses a similar system / principles to an FDG file.

This is also better then MonoBehaviours becuase its all C# based. No expensive get components calls or anything like that and to me it just makes sense. Not everything needs to be a component.

Entity system also removes the need for everything to be scene dependent / prefab based too. Which is also something I find incredibly stupid with unity.

### > Assets
All assets are loaded at runtime using AssetBundles. this provides many benifits such as everything not being scene dependent.

### > Callback System
Easy attribute based callback system, assign a method the ```[Callback("callback.name")]``` attribute and then run it using ```Callback.Run("callback.name");``` the string being the callback

### > Console System
Attribute based and easily extendable, use Debugging.Cmd or Debugging.Var attributes on propertys or methods for them to be used as a console command
