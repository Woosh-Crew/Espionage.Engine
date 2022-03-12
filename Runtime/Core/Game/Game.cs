using Espionage.Engine.Gamemodes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

namespace Espionage.Engine
{
	/// <summary>
	/// The Entry point for your game.
	/// Use this as your "GameManager".
	/// </summary>
	[Spawnable, Group( "Engine" )]
	public abstract class Game : IPackage
	{
		public Loader Loader { get; protected set; }
		public Splash Splash { get; protected set; }
		public Menu Menu { get; protected set; }

		// Class
		public Library ClassInfo { get; }

		protected Game()
		{
			ClassInfo = Library.Register( this );
		}

		~Game()
		{
			Library.Unregister( this );
		}

		// IPackage

		public abstract void OnReady();
		public abstract void OnShutdown();

		// Networking

		public virtual void Simulate( Client cl )
		{
			// Temp simulator.
			if ( cl.Pawn != null )
			{
				cl.Pawn.Simulate( cl );
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
			camSetup.Viewer = null;
			camSetup.Clipping = new( 0.1f, 700 );

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

		/// <summary>
		/// Override this if your camera needs custom components,
		/// such as Render pipeline specific components. 
		/// </summary>
		/// <param name="camera">
		/// The Main Camera created by Espionage.Engine, which is
		/// a singleton.
		/// </param>
		public virtual void OnCameraCreated( Camera camera )
		{
			// Setup Render Path
			camera.renderingPath = RenderingPath.DeferredShading;

			// Setup Post Processing
			var postProcessLayer = camera.gameObject.AddComponent<PostProcessLayer>();
			postProcessLayer.Init( UnityEngine.Resources.Load<PostProcessResources>( "PostProcessResources" ) );

			postProcessLayer.volumeTrigger = camera.transform;
			postProcessLayer.volumeLayer = LayerMask.GetMask( "TransparentFX" );
			postProcessLayer.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;

			var debug = camera.gameObject.AddComponent<PostProcessDebug>();
			debug.postProcessLayer = postProcessLayer;
		}

		//
		// Build Input
		//

		public virtual IControls.Setup BuildControls( IControls.Setup builder )
		{
			// If the Current Tripod can BuildInput, let it
			(LastTripod as IControls)?.Build( builder );

			// Now if the pawn can change input, let it.
			if ( Local.Pawn != null && Local.Pawn is IControls controls )
			{
				controls.Build( builder );
			}

			return builder;
		}

		//
		// Callbacks
		//

		[Function, Callback( "map.loaded" )]
		public virtual void OnMapLoaded() { }
	}
}
