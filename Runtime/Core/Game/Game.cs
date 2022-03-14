using Espionage.Engine.Gamemodes;
using Espionage.Engine.Tripods;
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

		// Networking & Game-loop

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

		/// <summary>
		/// <para>
		/// The gamemode is used for controlling game flow. Instead of jamming
		/// or your game specific logic into your game manager you'd make separate
		/// "Gamemodes".
		/// </para>
		/// <para>
		/// Gamemodes have callbacks for a lot of Game specific things, like when an
		/// actor took damage, actor got killed, etc
		/// </para>
		/// <example>
		/// You can use Gamemodes for Single-player, Death-match, etc.
		/// </example>
		/// </summary>
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

		private Gamemode _gamemode;

		//
		// Build Camera
		//

		private ITripod LastTripod { get; set; }

		/// <summary>
		/// Finds the Active tripod in the game. It goes from the
		/// client Tripod, down to the pawns tripod. Which ever
		/// tripod is returned, it'll use that tripod as the main
		/// camera controller.
		/// </summary>
		/// <returns>
		/// The active tripod that should be built and used as the
		/// main camera controller.
		/// </returns>
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

		/// <summary>
		/// Build camera will find the active tripod, and use it for
		/// controlling the main camera, you shouldn't need to override
		/// this, instead use <see cref="PostCameraSetup"/>.
		/// </summary>
		public virtual Tripod.Setup BuildCamera( Tripod.Setup camSetup )
		{
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

		/// <summary>
		/// PostCameraSetup is called to mutate the current Tripod.Setup
		/// pipeline. We use this for asking the pawn, viewmodels and
		/// modifiers to mutate the Tripod.Setup.
		/// </summary>
		protected virtual void PostCameraSetup( ref Tripod.Setup camSetup )
		{
			if ( Local.Pawn != null )
			{
				Local.Pawn.PostCameraSetup( ref camSetup );
			}

			Viewmodel.Apply( ref camSetup );
			Tripod.Modifier.Apply( ref camSetup );
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

			// Add support for flares
			camera.gameObject.AddComponent<FlareLayer>();

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

		/// <summary>
		/// Use build controls for mutating the current input pipeline.
		/// It'll go from the current tripod and down to the pawn for
		/// mutation.
		/// </summary>
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

		[Function, Callback( "cookies.saved" )]
		public virtual void OnCookiesSaved() { }
	}
}
