using System;
using System.Diagnostics;
using Espionage.Engine.Gamemodes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Espionage.Engine
{
	public class Shit : Game
	{
		public override string SplashScreen { get; }
		public override string MainMenu { get; }
	}

	/// <summary>
	/// The Entry point for your game. Use this as your "GameManager".
	/// </summary>
	[Spawnable, Group( "Engine" )]
	public abstract class Game : ILibrary, ICallbacks
	{
		// Mode

		public enum Mode { Offline, Online, Both }

		public virtual Mode Type => Mode.Offline;

		// Register

		public Library ClassInfo { get; }

		protected Game()
		{
			ClassInfo = Library.Database.Get( GetType() );
			Callback.Register( this );
		}

		~Game()
		{
			Callback.Unregister( this );
		}

		/// <summary>
		/// Called when the Engine & Game finishes initializing.
		/// </summary>
		public virtual void OnReady() { }

		/// <summary>
		/// Called when the Application is being shutdown.
		/// </summary>
		public virtual void OnShutdown() { }

		/// <summary>
		/// Called every application frame.
		/// </summary>
		public virtual void OnUpdate() { }

		//
		// Networking
		//

		internal void ClientJoined( Client client )
		{
			OnClientJoined( client );

			// TODO: Temp until we get networking down.
			OnClientReady( client );
		}

		internal void ClientDisconnected( Client client ) { OnClientDisconnect( client ); }
		internal void ClientReady( Client client ) { OnClientReady( client ); }

		protected virtual void OnClientJoined( Client client ) { }
		protected virtual void OnClientDisconnect( Client client ) { }
		protected virtual void OnClientReady( Client client ) { }

		//
		// Gamemode
		//

		/// <summary>
		/// The current gamemode in play.
		/// </summary>
		public Gamemode Gamemode { get; private set; }

		/// <summary>
		/// Switch the gamemode to a new one.
		/// </summary>
		/// <param name="gamemode">The new gamemode.</param>
		public void SwitchGamemode( Gamemode gamemode )
		{
			if ( !gamemode.Validate() )
			{
				Debugging.Log.Warning( $"Gamemode {gamemode.ClassInfo.Name} is not valid for map" );
				return;
			}

			// Finish and do Cleanup
			Gamemode.Finish();

			// Start new Gamemode
			Gamemode = gamemode;
			Gamemode.Begin();

			Callback.Run( "gamemodes.switched", true, gamemode );
		}

		//
		// Required Scenes
		//

		/// <summary>
		/// A Path to the Splash Screen Scene, this will be the first scene
		/// that is loaded at runtime. Espionage.Engine uses this for
		/// asset and engine initialization.
		/// </summary>
		public abstract string SplashScreen { get; }

		/// <summary>
		/// A Path to the Main Menu Scene
		/// </summary>
		public abstract string MainMenu { get; }

		//
		// Build Camera
		//

		private ICamera LastCamera { get; set; }

		protected virtual ICamera FindActiveCamera()
		{
			return null;
		}

		public ICamera.Setup BuildCamera( ICamera.Setup camSetup )
		{
			var cam = FindActiveCamera();

			if ( LastCamera != cam )
			{
				LastCamera?.Deactivated();
				LastCamera = cam;
				LastCamera?.Activated();
			}

			cam?.Build( ref camSetup );

			// if we have no cam, lets use the pawn's eyes directly
			PostCameraSetup( ref camSetup );

			return camSetup;
		}

		protected virtual void PostCameraSetup( ref ICamera.Setup camSetup ) { }
	}
}
