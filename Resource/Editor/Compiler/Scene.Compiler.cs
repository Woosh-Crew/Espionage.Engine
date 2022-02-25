using System;
using System.IO;
using System.Linq;
using Espionage.Engine.Editor;
using Espionage.Engine.Tools.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources.Editor
{
	[Title( "Map Compiler" ), Group( "Compiler" )]
	public class SceneCompiler : EditorTool
	{
		protected override void OnCreateGUI()
		{
			rootVisualElement.Add( new HeaderBar( "Map Compiler", "Compile your maps", null, "Header-Bottom-Border" ) );
		}

		// Menu Items

		[MenuItem( "Tools/Espionage.Engine/Compiler/Map Compiler", priority = -50 )]
		public static void OpenWindow()
		{
			GetWindow<SceneCompiler>();
		}

		[MenuItem( "Tools/Espionage.Engine/Compiler/Quick Compile Map _F6" )]
		public static void CompileActiveScene()
		{
			Compile( SceneManager.GetActiveScene().path, BuildTarget.StandaloneWindows );
		}

		// Compiler

		public static void Compile( string scenePath, params BuildTarget[] targets )
		{
			// Ask the user if they want to save the scene, if not don't export!
			var activeScene = SceneManager.GetActiveScene();
			var originalPath = activeScene.path;

			if ( activeScene.path == scenePath && !EditorSceneManager.SaveModifiedScenesIfUserWantsTo( new[] { SceneManager.GetActiveScene() } ) )
			{
				return;
			}

			var scene = EditorSceneManager.OpenScene( scenePath, OpenSceneMode.Single );

			var exportPath = $"Exports/{Library.Database.Get<Map>().Group}/{scene.name}/";

			// Track how long exporting took
			using ( Debugging.Stopwatch( "Level Compiled", true ) )
			{
				//
				// Export Level Processes
				//

				if ( Callback.Run<bool>( "compiler.sanity_check", scene )?.Any( e => e is false ) ?? false )
				{
					Debug.Log( "Sanity check failed" );
					return;
				}

				// Compile Preprocess. Allows anything to act as a preprocessor
				Callback.Run( "compiler.pre_process", scene );

				try
				{
					// Create the Map scene, we use this for preprocessing & exporting
					EditorSceneManager.SaveScene( scene, "Assets/Map.unity", true );
					AssetDatabase.Refresh();

					if ( !Directory.Exists( Path.GetFullPath( exportPath ) ) )
					{
						Directory.CreateDirectory( Path.GetFullPath( exportPath ) );
					}

					var extension = Library.Database.Get<AssetBundleMapProvider>().Components.Get<FileAttribute>().Extension ?? "map";

					var builds = new[]
					{
						new AssetBundleBuild()
						{
							assetNames = new[] { "Assets/Map.unity" },
							assetBundleName = $"{scene.name}.{extension}"
						}
					};

					// For each target build, build
					foreach ( var target in targets )
					{
						var bundle = BuildPipeline.BuildAssetBundles( exportPath, builds, BuildAssetBundleOptions.ChunkBasedCompression, target );

						if ( bundle is null )
						{
							EditorUtility.DisplayDialog( "ERROR", $"Map asset bundle compile failed. {target.ToString()}", "Okay" );
							return;
						}
					}

					// Shove Meta Data In
				}
				catch ( Exception e )
				{
					Debugging.Log.Exception( e );
				}
				finally
				{
					EditorSceneManager.OpenScene( originalPath );

					// Delete Level1, as its not needed anymore
					AssetDatabase.DeleteAsset( "Assets/Map.unity" );
					AssetDatabase.Refresh();
				}
			}
		}
	}
}
