using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine
{
	/// <summary>
	/// <para>
	/// The IGameProvider is the entry point for your game. This will be automatically created at runtime.
	/// </para>
	/// <para>
	/// Think of this as your game manager. Except you dont want all your logic flowing through this. Since
	/// singletons suck... We use the Game Provider to get your games AppID as well for initializing your games
	/// systems or really anything you need. You should 
	/// </para>
	/// </summary>
	public interface IGameProvider
	{
		uint AppId { get; }

		void OnReady();
		void OnShutdown();
		void OnLevelLoaded( Scene lastScene, Scene newScene /* We should pass our own level class */ );
	}
}
