using UnityEngine;

namespace Espionage.Engine
{
	[Spawnable]
	public abstract class Game : ILibrary, ICallbacks, IProject
	{
		public Library ClassInfo { get; }

		public Game()
		{
			ClassInfo = Library.Database.Get( GetType() );
			Callback.Register( this );
		}

		~Game()
		{
			Callback.Unregister( this );
		}

		//
		// Required Scenes
		//

		/// <summary>A Path to the Splash Screen Scene</summary>
		public abstract string SplashScreen { get; }

		/// <summary>A Path to the Main Menu Scene</summary>
		public abstract string MainMenu { get; }

		//
		// Required Methods
		//

		public abstract void OnReady();
		public abstract void OnShutdown();
		public abstract void OnCompile();
	}
}
