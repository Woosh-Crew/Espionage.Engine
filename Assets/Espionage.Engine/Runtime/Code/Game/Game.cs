using UnityEngine;
using Steamworks;

namespace Espionage.Engine
{
	[Manager( nameof( Initialize ), Layer = Layer.Runtime, Order = 500 )]
	public static class Game
	{
		public static IGameProvider Provider { get; }

		private static void Initialize()
		{
			SteamClient.Init( Provider?.AppId ?? 252490 );
			Provider?.Ready();

			// Weird shit for when the application should stop running

#if UNITY_EDITOR
			UnityEditor.EditorApplication.playModeStateChanged += ( e ) =>
			{
				if ( e is UnityEditor.PlayModeStateChange.ExitingPlayMode )
					Provider?.Shutdown();
			};
#else
			Application.quitting += () => Provider?.Shutdown();
#endif

		}
	}
}
