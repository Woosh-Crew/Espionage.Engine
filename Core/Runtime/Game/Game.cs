using Espionage.Engine.Gamemodes;

namespace Espionage.Engine
{
	/// <summary>
	/// The Entry point for your game. Use this as your "GameManager".
	/// </summary>
	[Spawnable, Group( "Engine" )]
	public abstract class Game : ILibrary, ICallbacks
	{
		public Splash Splash { get; protected set; }
		public Menu Menu { get; protected set; }

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

		internal void ClientReady( Client client )
		{
			if ( Gamemode != null )
			{
				Gamemode.OnClientReady( client );
			}

			OnClientReady( client );
		}

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
