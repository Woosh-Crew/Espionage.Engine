using System;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Espionage.Engine.Resources
{
	[Group( "Maps" ), Title( "UMAP File" ), File( Extension = "umap" )]
	public sealed class UMAP : IFile<Map, Scene>, IAsset
	{
		public Library ClassInfo { get; }

		public UMAP() { ClassInfo = Library.Register( this ); }
		~UMAP() { Library.Unregister( this ); }

		// Resource

		public FileInfo File { get; set; }
		public void Load( FileStream fileStream ) { }

		// Provider

		public Resource.IProvider<Map, Scene> Provider()
		{
			// This file is basically an Asset Bundle
			return new AssetBundleMapProvider( File );
		}

		// Compiler

	#if UNITY_EDITOR

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
			using ( Debugging.Stopwatch( "Map Compiled", true ) )
			{
				//
				// Export Level Processes
				//

				if ( Callback.Run<bool>( "compiler.sanity_check", scene )?.Any( e => e is false ) ?? false )
				{
					Debugging.Log.Info( "Sanity check failed" );
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

					var extension = Library.Database.Get<UMAP>().Components.Get<FileAttribute>().Extension;

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

						if ( bundle == null )
						{
							EditorUtility.DisplayDialog( "ERROR", $"Map asset bundle compile failed. {target.ToString()}", "Okay" );
							return;
						}
					}
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

	#endif
	}
}
