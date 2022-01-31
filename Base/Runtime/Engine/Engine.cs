using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine
{
	[Manager( nameof( Initialize ), Layer = Layer.Runtime | Layer.Editor, Order = 500 )]
	public static class Engine
	{
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
		// Layer
		//

		public static Scene Scene { get; private set; }

		private static void CreateEngineLayer()
		{
			// Create engine layer scene
			Scene = SceneManager.CreateScene( "Engine Layer" );
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

			// Setup Camera
			SetupCamera();
		}

		private static void OnShutdown()
		{
			Game?.OnShutdown();
		}

		//
		// Camera Building
		//

		private static Tripod.Setup _lastSetup;

		private static void SetupCamera()
		{
			if ( Game == null )
			{
				return;
			}

			// Build the camSetup, from game.
			_lastSetup = Game.BuildCamera( _lastSetup );

			// Get Camera Component
			CameraController.Instance.Finalise( _lastSetup );
		}
	}
}
