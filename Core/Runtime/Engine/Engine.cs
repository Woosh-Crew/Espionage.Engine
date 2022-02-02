using System.Collections.Generic;
using System.Linq;
using Espionage.Engine.Services;
using Espionage.Engine.Services.Camera;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine
{
	/// <summary>
	/// Espionage.Engine Entry Point. Initializes all its services, and sets up the Game.
	/// </summary>
	// [Manager( nameof( Initialize ), Layer = Layer.Runtime | Layer.Editor, Order = 500 )]
	public static class Engine
	{
		public static Game Game { get; private set; }

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize()
		{
			Callback.Run( "engine.initialize" );

			using ( Debugging.Stopwatch( "Engine / Game Ready", true ) )
			{
				// Setup Callbacks
				Application.quitting -= OnShutdown;
				Application.quitting += OnShutdown;

				CreateEngineLayer();
				SetupGame();

				// Tell Services we're ready
				foreach ( var service in Services.All )
				{
					service.OnReady();
				}

				// Frame Update
				Application.onBeforeRender -= OnFrame;
				Application.onBeforeRender += OnFrame;

				Game?.OnReady();
			}
		}

		private static void SetupGame()
		{
			// Setup Game
			var target = Library.Database.GetAll<Game>().FirstOrDefault( e => !e.Class.IsAbstract );

			if ( target is null )
			{
				Debugging.Log.Error( "Game couldn't be found. Make sure to make a class inherited from Game" );
				Callback.Run( "game.not_found" );
				return;
			}

			Game = Library.Database.Create<Game>( target.Class );
			Callback.Run( "game.ready" );

#if UNITY_EDITOR
			// Setup PlayerSettings based off of Project
			UnityEditor.PlayerSettings.productName = Game.ClassInfo.Title;
#endif
		}

		//
		// Services
		//

		public static IDatabase<IService> Services { get; } = new ServiceDatabase();

		private class ServiceDatabase : IDatabase<IService>
		{
			public IEnumerable<IService> All => _services;

			private readonly List<IService> _services = new() { new CameraService() };

			public void Add( IService item )
			{
				_services.Add( item );
			}

			public bool Contains( IService item )
			{
				return _services.Contains( item );
			}

			public void Remove( IService item )
			{
				_services.Remove( item );
			}

			public void Clear()
			{
				_services.Clear();
			}
		}

		//
		// Layer
		//

		/// <summary>
		/// The Engine Layer scene. This should never be unloaded.
		/// </summary>
		public static Scene Scene { get; private set; }

		private static void CreateEngineLayer()
		{
			// Create engine layer scene
			Scene = SceneManager.CreateScene( "Engine" );
			Callback.Run( "engine.layer_created" );
		}

		/// <summary>
		/// Adds an Object to the Engine Layer scene.
		/// </summary>
		/// <param name="gameObject">The GameObject to add</param>
		public static void AddToLayer( GameObject gameObject )
		{
			SceneManager.MoveGameObjectToScene( gameObject, Scene );
		}

		//
		// Callbacks
		//

		private static void OnFrame()
		{
			foreach ( var service in Services.All )
			{
				service.OnUpdate();
			}
		}

		private static void OnShutdown()
		{
			foreach ( var service in Services.All )
			{
				service.OnShutdown();
			}

			Game?.OnShutdown();
		}
	}
}
