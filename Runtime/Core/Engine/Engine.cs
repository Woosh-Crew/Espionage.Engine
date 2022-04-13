using Espionage.Engine.Services;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine
{
	/// <summary>
	/// Espionage.Engine Entry Point.
	/// Initializes all its services, and sets up the Game.
	/// </summary>
	[Library, Group( "Engine" )]
	public static class Engine
	{
		/// <summary>
		/// The Current Game that is in Session, this will
		/// be automatically created when the game launches, and
		/// will be based off the game you exported from	
		/// the packager.
		/// </summary>
		public static Game Game { get; private set; }

		/// <summary>
		/// Services are hooks attached to Espionage.Engine. This
		/// allows us to fire and forget stuff that services would need.
		/// such as, Update, PostUpdate, Shutdown, etc. Think of it as
		/// micro managers. So Camera Managers, Input Mangers, etc.
		/// </summary>
		public static Service.Database Services { get; private set; }

		/// <summary>
		/// The Engine Layer scene. Use this scene
		/// for persisting objects across map changes.
		/// This scene should never be unloaded.
		/// </summary>
		public static Scene Scene { get; private set; }

		/// <summary>
		/// The Main Camera Espionage.Engine creates. We recommend you
		/// cache this as this will do a services Get() call, which
		/// creates garbage (due to LINQ).
		/// </summary>
		public static Camera Camera => Services.Get<CameraService>().Camera;

		//
		// Initialization
		//

		[RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.AfterSceneLoad )]
		private static void Initialize_RuntimePostScene()
		{
			using var _ = Debugging.Stopwatch( "Engine / Game Ready", true );

			// Find Game
			if ( Game == null && !SetupGame() )
			{
				Debugging.Log.Error( "Game couldn't be found. Make sure to make a class inherited from Game" );
				return;
			}

			UpdatePlayerLoop( false );
			Services = new();

			// Create engine layer scene
			Scene = SceneManager.CreateScene( "Engine Layer" );

			Local.Client = new Client( "Local" );
			Callback.Run( "engine.getting_ready" );

			Services.Ready();
			Game.OnReady();

			Application.quitting += OnShutdown;

			Callback.Run( "engine.ready" );
		}

		#if UNITY_EDITOR

		[InitializeOnLoadMethod]
		private static void Initialize_Editor()
		{
			// Find Game
			if ( Game == null && !SetupGame() )
			{
				Debugging.Log.Error( "Game couldn't be found. Make sure to make a class inherited from Game" );
				return;
			}

			Callback.Run( "editor.game_constructed" );
			Callback.Run( "editor.ready" );
		}

		#endif

		private static bool SetupGame()
		{
			var target = Library.Database.Find<Game>();

			if ( target == null )
			{
				Callback.Run( "game.not_found" );
				return false;
			}

			Game = Library.Create<Game>( target.Info );

			Callback.Run( "game.ready" );

			Debugging.Log.Info( $"Using {Game.ClassInfo.Title} as the Game, [{Game.ClassInfo.Name}]" );

			return true;
		}

		//
		// Layer
		//

		private static void UpdatePlayerLoop( bool remove = false )
		{
			var loop = PlayerLoop.GetCurrentPlayerLoop();

			for ( var i = 0; i < loop.subSystemList.Length; ++i )
			{
				// Frame Update
				if ( loop.subSystemList[i].type == typeof( Update ) )
				{
					if ( remove )
					{
						loop.subSystemList[i].updateDelegate -= OnUpdate;
					}
					else
					{
						loop.subSystemList[i].updateDelegate += OnUpdate;
					}
				}

				if ( loop.subSystemList[i].type == typeof( PostLateUpdate ) )
				{
					if ( remove )
					{
						loop.subSystemList[i].updateDelegate -= OnPostUpdate;
					}
					else
					{
						loop.subSystemList[i].updateDelegate += OnPostUpdate;
					}
				}
			}

			PlayerLoop.SetPlayerLoop( loop );
		}

		//
		// Callbacks
		//

		private static void OnUpdate()
		{
			Services.Update();
			Callback.Run( "app.frame" );

			foreach ( var client in Client.All )
			{
				client.Simulate();
			}
		}

		private static void OnPostUpdate()
		{
			Services.PostUpdate();
		}

		public static bool IsQuitting { get; private set; }

		private static void OnShutdown()
		{
			IsQuitting = true;

			UpdatePlayerLoop( true );

			Services.Shutdown();
			Game.OnShutdown();
		}
	}
}
