using System;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
using Espionage.Engine.Resources.Binders;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Espionage.Engine.Resources.Formats
{
	[Title( "Unity Map File" ), File( Extension = "umap" )]
	public sealed partial class UMAP : AssetBundleFile { }

	#if UNITY_EDITOR

	public partial class UMAP : ICompiler<SceneAsset>
	{
		public void Compile( string scenePath )
		{
			// Ask the user if they want to save the scene, if not don't export!
			var activeScene = SceneManager.GetActiveScene();
			var originalPath = activeScene.path;

			if ( activeScene.path == scenePath && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() )
			{
				Dev.Log.Warning( "Not compiling, User didn't want to save." );
				return;
			}

			var scene = EditorSceneManager.OpenScene( scenePath, OpenSceneMode.Single );
			var exportPath = $"Exports/{ClassInfo.Group}/";

			// Track how long exporting took
			using ( Dev.Stopwatch( "Map Compiled", true ) )
			{
				if ( Callback.Run<bool>( "compiler.sanity_check", scene )?.Any( e => e is false ) ?? false )
				{
					Dev.Log.Info( "Sanity check failed" );
					return;
				}

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
					var builds = new[] { new AssetBundleBuild() { assetNames = new[] { "Assets/Map.unity" }, assetBundleName = $"{scene.name}.{extension}" } };

					var bundle = BuildPipeline.BuildAssetBundles( exportPath, builds, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows );

					if ( bundle == null )
					{
						EditorUtility.DisplayDialog( "Map Failed to Compile", "Map asset bundle compile failed.", "Okay" );
						return;
					}

					Files.Delete( $"assets://{ClassInfo.Group}", "manifest", "" );
				}
				catch ( Exception e )
				{
					Dev.Log.Exception( e );
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

	#endif
}
