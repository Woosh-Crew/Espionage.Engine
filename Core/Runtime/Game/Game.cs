using Espionage.Engine.Gamemodes;
using UnityEngine;

namespace Espionage.Engine
{
	public class TestGame : Game { }

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

		public virtual void OnReady() { }
		public virtual void OnShutdown() { }

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

		public Gamemode Gamemode { get; private set; }

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

			Callback.Run( "gamemodes.switched", gamemode );
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

			PreCameraSetup( ref camSetup );

			cam?.Build( ref camSetup );

			// if we have no cam, lets use the pawn's eyes directly
			PostCameraSetup( ref camSetup );

			return camSetup;
		}

		private Vector3 targetPos;

		protected virtual void PreCameraSetup( ref ICamera.Setup camSetup )
		{
			var input = Local.Client.Input;
			camSetup.Rotation = input.Rotation;

			var vel = camSetup.Rotation * Vector3.forward * input.Forward + camSetup.Rotation * Vector3.left * input.Horizontal;

			if ( Input.GetKey( KeyCode.Space ) || Input.GetKey( KeyCode.E ) )
			{
				vel += Vector3.up * 1;
			}

			if ( Input.GetKey( KeyCode.LeftControl ) || Input.GetKey( KeyCode.Q ) )
			{
				vel += Vector3.down * 1;
			}

			vel = vel.normalized * 20;

			if ( Input.GetKey( KeyCode.LeftShift ) )
			{
				vel *= 5.0f;
			}

			if ( Input.GetKey( KeyCode.LeftAlt ) )
			{
				vel *= 0.2f;
			}

			targetPos += vel * Time.deltaTime;

			camSetup.Position = Vector3.Lerp( camSetup.Position, targetPos, 5 * Time.deltaTime );
		}

		protected virtual void PostCameraSetup( ref ICamera.Setup camSetup ) { }
	}
}
