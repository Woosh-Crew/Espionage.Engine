using Espionage.Engine;
using UnityEngine;
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
		/// The current project that is in sSession, this will
		/// be automatically created when the application launches.
		/// </summary>
		public static Project Project { get; private set; }

		/// <summary>
		/// Modules are hooks attached to Espionage.Engine. This
		/// allows us to fire and forget logic that needs to run
		/// in the background. We use this for Tripods and Controls
		/// </summary>
		public static Module.Registry Modules { get; private set; }

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
		public static Camera Camera => Modules.Get<CameraModule>().Camera;

		/// <summary>
		/// Bootstrap is how Espionage.Engine connects with Unity. Hooks into
		/// its low level player loop, adding update methods as well as applications
		/// quitting hooks.
		/// </summary>
		public static Bootstrap Bootstrap { get; } = new( OnShutdown, OnUpdate );

		// Initialization

		[RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.AfterSceneLoad )]
		private static void Initialize_RuntimePostScene()
		{
			using var _ = Debugging.Stopwatch( "Engine / Game Ready", true );

			// Find Game
			if ( Project == null && !Setup() )
			{
				Debugging.Log.Error( "Game couldn't be found. Make sure to make a class inherited from Game" );
				return;
			}

			Bootstrap.Inject();
			
			Local.Client = new( "Local" );
			
			// Create engine layer scene
			Scene = SceneManager.CreateScene( "Engine Layer" );
			
			Modules = new();
			Callback.Run( "engine.getting_ready" );

			Modules.Ready();
			Project.OnReady();

			Callback.Run( "engine.ready" );
		}

		#if UNITY_EDITOR

		[InitializeOnLoadMethod]
		private static void Initialize_Editor()
		{
			// Find Game
			if ( Project == null && !Setup() )
			{
				Debugging.Log.Error( "Project couldn't be found. Make sure to make a class inherited from Project" );
				return;
			}

			Callback.Run( "editor.game_constructed" );
			Callback.Run( "editor.ready" );
		}

		#endif

		private static bool Setup()
		{
			var target = Library.Database.Find<Project>();

			if ( target == null )
			{
				return false;
			}

			var project = Library.Create<Project>( target );
			Project = project;

			Callback.Run( "game.ready" );
			Debugging.Log.Info( $"Using {Project.ClassInfo.Title} as the Project, [{Project.ClassInfo.Name}]" );

			return true;
		}

		// Frame Loop

		private static void OnUpdate()
		{
			Modules.Frame();
			Project.OnUpdate();

			OnTick(); // We will change this to a fixed rate in the future

			Callback.Run( "app.frame" );
		}

		private static void OnTick()
		{
			foreach ( var client in Client.All )
			{
				client.Simulate();
			}
		}

		// Quit

		public static bool IsQuitting { get; private set; }

		private static void OnShutdown()
		{
			IsQuitting = true;

			Bootstrap.Remove();
			Modules.Shutdown();
			Project.OnShutdown();
		}
	}
}
