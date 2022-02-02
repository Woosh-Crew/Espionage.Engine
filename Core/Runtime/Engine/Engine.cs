using System.Collections.Generic;
using System.Linq;
using Espionage.Engine.Services;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine
{
	/// <summary>
	/// Espionage.Engine Entry Point. Initializes all its systems, and sets up the Game.
	/// </summary>
	[Manager( nameof( Initialize ), Layer = Espionage.Engine.Layer.Runtime | Espionage.Engine.Layer.Editor, Order = 500 )]
	public static class Engine
	{
		/// <summary>
		/// The Current game that is in session.
		/// </summary>
		public static Game Game { get; private set; }

		private static void Initialize()
		{
			using ( Debugging.Stopwatch( "Engine / Game Ready", true ) )
			{
				// Setup Callbacks
				Application.quitting -= OnShutdown;
				Application.quitting += OnShutdown;

				// Frame Update
				Application.onBeforeRender -= OnFrame;
				Application.onBeforeRender += OnFrame;

				// Setup Game
				var target = Library.Database.GetAll<Game>().FirstOrDefault( e => !e.Class.IsAbstract );

				if ( target is null )
				{
					Debugging.Log.Warning( "Game couldn't be found." );
					Callback.Run( "game.not_found" );
					return;
				}

				Game = Library.Database.Create<Game>( target.Class );

				// Ready Up Project
				Callback.Run( "game.ready" );
				Game.OnReady();
			}

			// Setup PlayerSettings based off of Project
#if UNITY_EDITOR
			PlayerSettings.productName = Game.ClassInfo.Title;

			if ( Game.ClassInfo.Components.TryGet<CompanyAttribute>( out var item ) )
			{
				PlayerSettings.companyName = item.Company;
			}
#endif
		}

		[RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad )]
		private static void Runtime_Initialize()
		{
			CreateEngineLayer();
		}

		//
		// Services
		//

		public static IDatabase<IService> Services { get; } = new ServiceDatabase();

		private class ServiceDatabase : IDatabase<IService>
		{
			public IEnumerable<IService> All => _services;

			private List<IService> _services;

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
		public static Scene Layer { get; private set; }

		private static void CreateEngineLayer()
		{
			// Create engine layer scene
			Layer = SceneManager.CreateScene( "Engine" );
			Callback.Run( "engine.layer_created" );
		}

		/// <summary>
		/// Adds an Object to the Engine Layer scene.
		/// </summary>
		/// <param name="gameObject">The GameObject to add</param>
		public static void AddToLayer( GameObject gameObject )
		{
			SceneManager.MoveGameObjectToScene( gameObject, Layer );
		}

		//
		// Callbacks
		//

		private static void OnFrame()
		{
			// Guard clause just in case.
			if ( !Application.isPlaying )
			{
				return;
			}

			foreach ( var service in Services.All )
			{
				service.OnUpdate();
			}

			// Setup Camera
			SetupCamera();
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
