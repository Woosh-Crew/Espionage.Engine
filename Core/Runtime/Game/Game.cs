using Espionage.Engine.Gamemodes;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// The Entry point for your game.
	/// Use this as your "GameManager".
	/// </summary>
	[Spawnable, Group( "Engine" )]
	public abstract class Game : ILibrary
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

		private ITripod LastTripod { get; set; }

		protected virtual ITripod FindActiveCamera()
		{
			if ( Local.Client.Tripod != null )
			{
				return Local.Client.Tripod;
			}

			if ( Local.Client.Pawn != null && Local.Client.Pawn.Tripod != null )
			{
				return Local.Client.Pawn.Tripod;
			}

			return null;
		}

		public virtual ITripod.Setup BuildCamera( ITripod.Setup camSetup )
		{
			// Default FOV
			camSetup.FieldOfView = 68;

			var cam = FindActiveCamera();

			if ( LastTripod != cam )
			{
				LastTripod?.Deactivated();
				LastTripod = cam;
				LastTripod?.Activated( ref camSetup );
			}

			LastTripod?.Build( ref camSetup );
			PostCameraSetup( ref camSetup );

			return camSetup;
		}

		protected virtual void PostCameraSetup( ref ITripod.Setup camSetup )
		{
			if ( Local.Pawn != null )
			{
				Local.Pawn.PostCameraSetup( ref camSetup );
			}

			Viewmodel.Apply( ref camSetup );
			ITripod.Modifier.Apply( ref camSetup );
		}

		/// <summary> Override this if your camera needs custom components </summary>
		/// <param name="camera"> The Main Camera </param>
		public virtual void OnCameraCreated( Camera camera ) { }

		//
		// Build Input
		//

		public virtual IControls.Setup BuildControls( IControls.Setup builder )
		{
			// If the Current Tripod can BuildInput, let it
			(LastTripod as IControls)?.Build( ref builder );

			// Now if the pawn can change input, let it.
			if ( Local.Pawn != null && Local.Pawn is IControls controls )
			{
				controls.Build( ref builder );
			}

			return builder;
		}
	}
}
