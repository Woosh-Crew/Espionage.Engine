using System.Collections.Generic;
using System.Linq;
using Espionage.Engine.Services;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace Espionage.Engine
{
	/// <summary>
	/// Espionage.Engine Entry Point.
	/// Initializes all its services, and sets up the Game.
	/// </summary>
	[Library, Group( "Engine" ), Manager( nameof( Initialize_Runtime ), Layer = Layer.Runtime, Order = 600 )]
	public static class Engine
	{
		public static Game Game { get; private set; }

		private static void Initialize_Runtime()
		{
			using ( Debugging.Stopwatch( "Engine / Game Ready", true ) )
			{
				if ( Game == null && !SetupGame() )
				{
					Debugging.Log.Error( "Game couldn't be found. Make sure to make a class inherited from Game" );
					return;
				}

				Debugging.Log.Info( $"Using {Game?.ClassInfo.Title} as the Game" );
				CreateEngineLayer();
				Services = new Database();

				// TODO: THIS IS TEMP
				Local.Client = Client.Create( "Local" );

				// Tell Services we're ready
				foreach ( var service in Services.All )
				{
					service.OnReady();
				}

				Game?.OnReady();

				Callback.Run( "engine.ready" );
			}
		}

		private static bool SetupGame()
		{
			var target = Library.Database.Find<Game>();

			if ( target is null )
			{
				Callback.Run( "game.not_found" );
				return false;
			}

			Game = Library.Database.Create<Game>( target.Class );
			Callback.Run( "game.ready" );

			return true;
		}

		//
		// Services
		//

		public static Database Services { get; private set; }

		public class Database : IDatabase<IService>
		{
			public IEnumerable<IService> All => _services;

			private readonly List<IService> _services = new();

			public Database()
			{
				foreach ( var service in Library.Database.GetAll<IService>().Where( e => !e.Class.IsAbstract ) )
				{
					Add( Library.Database.Create<IService>( service.Class ) );
				}

				_services = _services.OrderBy( e => e.ClassInfo.Components.Get<OrderAttribute>()?.Order ?? 10 ).ToList();
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
				item.Dispose();
			}

			public void Clear()
			{
				foreach ( var service in _services )
				{
					service.Dispose();
				}

				_services.Clear();
			}

			public int Length => _services.Count;

			public T Get<T>() where T : class, IService
			{
				return All.FirstOrDefault( e => e is T ) as T;
			}

			public bool Has<T>() where T : class, IService
			{
				return All.OfType<T>().Any();
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
			// Create Update Loop
			// Jake: I think this is stupid?
			var loop = PlayerLoop.GetCurrentPlayerLoop();
			for ( var i = 0; i < loop.subSystemList.Length; ++i )
			{
				// Frame Update
				if ( loop.subSystemList[i].type == typeof( Update ) )
				{
					Debugging.Log.Info( "Hooking Update" );
					loop.subSystemList[i].updateDelegate += OnUpdate;
				}

				// Physics Update
				if ( loop.subSystemList[i].type == typeof( FixedUpdate ) )
				{
					loop.subSystemList[i].updateDelegate += OnPhysicsUpdate;
				}
			}

			PlayerLoop.SetPlayerLoop( loop );

			// Create engine layer scene
			Scene = SceneManager.CreateScene( "Engine Layer" );
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
			if ( !Application.isPlaying )
			{
				return;
			}

			foreach ( var service in Services.All )
			{
				service.OnUpdate();
			}

			Callback.Run( "application.frame" );

			// More temp - this should 
			// Be called at an engine level
			Game.Simulate( Local.Client );
		}

		private static void OnPhysicsUpdate()
		{
			Callback.Run( "physics.frame" );
		}

		private static void OnShutdown()
		{
			foreach ( var service in Services.All )
			{
				service.OnShutdown();
			}

			Game?.OnShutdown();

			Callback.Run( "application.quit" );
		}
	}
}
