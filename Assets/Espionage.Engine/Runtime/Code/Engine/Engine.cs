using UnityEngine;
using Steamworks;
using UnityEngine.SceneManagement;

namespace Espionage.Engine
{
	[Manager( nameof( Initialize ), Layer = Layer.Runtime, Order = 500 )]
	public static class Engine
	{
		public static IGameProvider Game { get; }

		private static void Initialize()
		{
			SteamClient.Init( Game?.AppId ?? 252490 );

			// Weird shit for when the application should stop running

#if UNITY_EDITOR
			UnityEditor.EditorApplication.playModeStateChanged += ( e ) =>
			{
				if ( e is UnityEditor.PlayModeStateChange.ExitingPlayMode )
					Game?.OnShutdown();
			};
#else
			Application.quitting += () => Game?.Shutdown();
#endif

			// Init Scene Mangement
			SceneManager.activeSceneChanged += Game.OnLevelLoaded;

			// Game is now ready for use
			Game?.OnReady();
		}
	}
}
