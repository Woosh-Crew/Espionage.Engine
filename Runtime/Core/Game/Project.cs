using Espionage.Engine.Gamemodes;
using Espionage.Engine.Resources;
using UnityEngine;

namespace Espionage.Engine
{
	[Spawnable, Group( "Engine" )]
	public abstract partial class Project : ILibrary
	{
		public Library ClassInfo { get; }
		public Splash Splash { get; protected set; }

		public Project()
		{
			ClassInfo = Library.Register( this );
			Splash = new( string.Empty, 3 );
		}

		// Callbacks

		public abstract void OnReady();
		protected abstract void OnSetup( ref Scheme scheme );
		public virtual void OnShutdown() { }
		public virtual void OnUpdate() { }

		// Networking & Game-loop

		public virtual void Simulate( Client cl )
		{
			// Temp Simulator
			(cl.Pawn ? cl.Pawn : null)?.Simulate( cl );
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
				(_gamemode ? _gamemode : null)?.Finish();
				_gamemode = value;
				(_gamemode ? _gamemode : null)?.Begin();

				Callback.Run( "gamemodes.switched" );
			}
		}

		// Build Tripod

		protected ITripod LastTripod { get; set; }

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
		protected virtual ITripod FindActiveTripod()
		{
			if ( Local.Client.Tripod.Peek() != null )
			{
				return Local.Client.Tripod.Peek();
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
		internal Tripod.Setup BuildTripod( Tripod.Setup camSetup )
		{
			var cam = FindActiveTripod();

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
			camSetup.Viewmodel.FieldOfView = camSetup.FieldOfView;

			if ( Local.Pawn != null && LastTripod == Local.Pawn.Tripod )
			{
				Local.Pawn.PostCameraSetup( ref camSetup );
			}

			// First Person
			Viewmodel.Apply( ref camSetup );

			// Camera Effects
			Tripod.Effect.Apply( ref camSetup );
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
		}

		//
		// Controls
		//

		internal Controls.Setup BuildControls( Controls.Setup builder )
		{
			PostControlSetup( builder );
			return builder;
		}

		internal Scheme SetupControls()
		{
			// Default Scheme
			var scheme = new Scheme()
			{
				// Developer
				{ "Dev.Toggle", KeyCode.F1 },
				{ "Dev.Noclip", KeyCode.F3 },

				// Gameplay
				{ "Shoot", KeyCode.Mouse0 },
				{ "Shoot.Alt", KeyCode.Mouse1 },
				{ "Interact", KeyCode.F },
				{ "Switch.Tripod", KeyCode.C }
			};

			OnSetup( ref scheme );
			return scheme;
		}

		/// <summary>
		/// Use PostControlSetup for mutating the current input pipeline.
		/// It'll go from the current tripod and down to the pawn for
		/// mutation.
		/// </summary>
		protected virtual void PostControlSetup( Controls.Setup setup )
		{
			// If the Current Tripod can BuildInput, let it
			(LastTripod as IControls)?.Build( setup );

			// Now if the pawn can change input, let it.
			if ( Local.Pawn != null )
			{
				((IControls)Local.Pawn).Build( setup );
			}
		}
	}
}
