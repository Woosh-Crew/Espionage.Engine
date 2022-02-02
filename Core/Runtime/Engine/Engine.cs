using System;
using System.Collections.Generic;
using System.Linq;
using Espionage.Engine.Internal;
using Espionage.Engine.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine
{
	/// <summary>
	/// Espionage.Engine Entry Point. Initializes all its services, and sets up the Game.
	/// </summary>
	[Manager( nameof( Initialize ), Layer = Layer.Runtime, Order = 600 )]
	public static class Engine
	{
		public static Game Game { get; private set; }

		private static void Initialize()
		{
			using ( Debugging.Stopwatch( "Engine / Game Ready", true ) )
			{
				CreateEngineLayer();

				if ( !SetupGame() )
				{
					Debugging.Log.Error( "Game couldn't be found. Make sure to make a class inherited from Game" );
					return;
				}

				Services = new ServiceDatabase();

				// Setup Callbacks
				Application.quitting -= OnShutdown;
				Application.quitting += OnShutdown;


				// Tell Services we're ready
				foreach ( var service in Services.All )
				{
					service.OnReady();
				}

				// Frame Update
				Application.onBeforeRender -= OnUpdate;
				Application.onBeforeRender += OnUpdate;

				Game?.OnReady();
			}
		}

		private static bool SetupGame()
		{
			// Setup Game
			var target = Library.Database.GetAll<Game>().FirstOrDefault( e => !e.Class.IsAbstract );

			if ( target is null )
			{
				Callback.Run( "game.not_found" );
				return false;
			}

			Game = Library.Database.Create<Game>( target.Class );
			Callback.Run( "game.ready" );

#if UNITY_EDITOR
			// Setup PlayerSettings based off of Project
			UnityEditor.PlayerSettings.productName = Game.ClassInfo.Title;
#endif

			return true;
		}

		//
		// Services
		//

		public static IDatabase<IService> Services { get; private set; }

		private class ServiceDatabase : IDatabase<IService>
		{
			public IEnumerable<IService> All => _services;

			private readonly List<IService> _services = new() { };

			public ServiceDatabase()
			{
				// Cache all Services
				var types = AppDomain.CurrentDomain.GetAssemblies()
					.Where( Utility.IgnoreIfNotUserGeneratedAssembly )
					.SelectMany( e => e.GetTypes()
						.Where( ( type ) => type.HasInterface<IService>() && !type.IsAbstract ) );

				foreach ( var type in types )
				{
					Add( (IService)Activator.CreateInstance( type ) );
				}
			}

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

		private static void OnUpdate()
		{
			foreach ( var service in Services.All )
			{
				service.OnUpdate();
			}

			Game?.OnUpdate();
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
