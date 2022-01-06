using System;
using Steamworks;
using UnityEngine;

namespace Espionage.Engine
{
	public sealed class Game : Entity, IGameProvider
	{
		//
		// Espionage.Engine Entry Point
		//

		[RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.AfterSceneLoad )]
		private static void Initialization()
		{
			Main = Library.Accessor.Create<Game>();
			IGameProvider.Main = Main;
			DontDestroyOnLoad( Main );

			// Shutdown depending on if were in the editor or not.

#if UNITY_EDITOR
			UnityEditor.EditorApplication.playModeStateChanged += ( e ) =>
			{
				if ( e is UnityEditor.PlayModeStateChange.ExitingPlayMode )
					Main.Shutdown();
			};
#else
			Application.quitting += Main.Shutdown;
#endif
		}

		//
		// Instance
		//

		public static Game Main { get; private set; }

		private void Awake()
		{
			if ( Main is null )
				return;

			Debug.LogError( "Trying to creating another instance of Game" );
			Destroy( this );
		}

		private void Start()
		{
			Debug.Log( $"Initializing {Application.productName}" );

			// Initialize Steam
			try
			{
				SteamClient.Init( Global.AppId );
			}
			catch ( Exception e )
			{
				Debug.LogError( e );
			}
		}

		private void Update()
		{
			// Steam Callbacks
			SteamClient.RunCallbacks();
		}

		private void Shutdown()
		{
			Debug.Log( $"Shuting Down {Application.productName}" );

			// Steam stop running
			SteamClient.Shutdown();
		}
	}
}
