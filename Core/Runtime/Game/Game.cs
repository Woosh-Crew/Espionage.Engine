using Espionage.Engine.Gamemodes;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// The Entry point for your game. Use this as your "GameManager".
	/// </summary>
	[Spawnable, Group( "Engine" )]
	public abstract class Game : ILibrary, ICallbacks
	{
		public Loader Loader { get; protected set; }
		public Splash Splash { get; protected set; }
		public Menu Menu { get; protected set; }

		// Class

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

		// Required

		public abstract void OnReady();
		public abstract void OnShutdown();

		// Networking

		public virtual void Simulate( Client client )
		{
			// Temp simulator.
			if ( Local.Pawn != null )
			{
				Local.Pawn.Simulate( client );
			}
		}

		//
		// Gamemode
		//

		private Gamemode _gamemode;

		public Gamemode Gamemode
		{
			get => _gamemode;
			set
			{
				if ( value != null && !value.Validate() )
				{
					Debugging.Log.Warning( $"Gamemode {value.ClassInfo.Name} is not valid for map" );
					return;
				}

				// Finish and do Cleanup
				if ( _gamemode != null )
				{
					_gamemode.Finish();
				}

				_gamemode = value;

				if ( _gamemode != null )
				{
					_gamemode.Begin();
				}

				Callback.Run( "gamemodes.switched" );
			}
		}

		//
		// Build Camera
		//

		private ICamera LastCamera { get; set; }

		protected virtual ICamera FindActiveCamera()
		{
			if ( Local.Client.Camera != null )
			{
				return Local.Client.Camera;
			}

			if ( Local.Client.Pawn != null && Local.Client.Pawn.Tripod != null )
			{
				return Local.Client.Pawn.Tripod;
			}

			return null;
		}

		public ICamera.Setup BuildCamera( ICamera.Setup camSetup )
		{
			var cam = FindActiveCamera();

			if ( LastCamera != cam )
			{
				LastCamera?.Deactivated();
				LastCamera = cam;
				LastCamera?.Activated( ref camSetup );
			}

			PreCameraSetup( ref camSetup );

			LastCamera?.Build( ref camSetup );

			PostCameraSetup( ref camSetup );

			return camSetup;
		}

		protected virtual void PreCameraSetup( ref ICamera.Setup camSetup ) { }

		protected virtual void PostCameraSetup( ref ICamera.Setup camSetup ) { }
	}
}
