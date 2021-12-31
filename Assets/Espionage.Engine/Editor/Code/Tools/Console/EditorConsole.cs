using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Espionage.Engine.Internal
{
	public static partial class EditorConsole
	{
		[InitializeOnLoadMethod]
		private static void Initialize()
		{
			// The way that this works is that when we are in Unity
			// we want to use the EditorConsole Provider, so we have
			// commands only accessible through unity...
			Console.Provider = new EditorConsoleProvider();
			Console.Initialize();

			// Initialize CommandLine UI
		}

		//
		// Commands
		//

		[EditorConsole.Cmd( "test" )]
		private static void CmdNullablesTest( string preset = null )
		{
			Debug.Log( $"Worked - {preset}" );
		}

		[EditorConsole.Cmd( "editor.new_scene" )]
		private static void CmdNewScene( string preset = null )
		{
			if ( EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() )
			{
				NewSceneSetup setup = NewSceneSetup.DefaultGameObjects;

				// Parse preset
				if ( preset is "empty" )
					setup = NewSceneSetup.EmptyScene;

				EditorSceneManager.NewScene( setup, NewSceneMode.Single );
			}
		}
	}
}
