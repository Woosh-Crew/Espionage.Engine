using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Espionage.Engine.Resources
{
	[CreateAssetMenu( fileName = "Map", menuName = "Map", order = 0 ), File( Extension = "map" ), Group( "Maps" )]
	public sealed class MapReference : Asset
	{
		public string title;
		public string description;
		public Texture2D icon;

		//
		// Editor Only
		//

#if UNITY_EDITOR

		public SceneAsset sceneAsset;
		public BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.ChunkBasedCompression;

		public override bool CanCompile()
		{
			return sceneAsset != null;
		}

		public override void Compile( params BuildTarget[] targets )
		{
			var lastActiveScene = SceneManager.GetActiveScene().path;
			var sceneAssetPath = AssetDatabase.GetAssetPath( sceneAsset );

			// Ask the user if they want to save the scene, if not don't export!
			if ( !EditorSceneManager.SaveModifiedScenesIfUserWantsTo( new[] { SceneManager.GetActiveScene() } ) )
			{
				return;
			}

			var processedFileName = string.IsNullOrEmpty( name ) ? sceneAsset.name : name;
			var exportPath = $"Exports/{Library.Database.Get<Map>().Group}/{processedFileName}/";

			// Track how long exporting took
			using ( Debugging.Stopwatch( "Level Compiled", true ) )
			{
				//
				// Export Level Processes
				//

				var scene = EditorSceneManager.OpenScene( sceneAssetPath );

				if ( Callback.Run<bool>( "compiler.sanity_check", scene )?.Any( e => e is false ) ?? false )
				{
					Debug.Log( "Sanity check failed" );
					return;
				}

				// Compile Preprocess. Allows anything to act as a preprocessor
				Callback.Run( "compiler.pre_process", scene );

				try
				{
					// Create the Level1 scene, we use this for preprocessing & exporting
					EditorSceneManager.SaveScene( scene, "Assets/Map.unity", true );
					AssetDatabase.Refresh();

					if ( !Directory.Exists( Path.GetFullPath( exportPath ) ) )
					{
						Directory.CreateDirectory( Path.GetFullPath( exportPath ) );
					}

					var extension = Library.Database.Get<Map>().Components.Get<FileAttribute>().Extension ?? "map";

					var builds = new[]
					{
						new AssetBundleBuild()
						{
							assetNames = new[] { "Assets/Map.unity" },
							assetBundleName = $"{processedFileName}.{extension}"
						}
					};

					// For each target build, build
					foreach ( var target in targets )
					{
						var bundle = BuildPipeline.BuildAssetBundles( exportPath, builds, buildOptions, target );

						if ( bundle is null )
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
					EditorSceneManager.OpenScene( lastActiveScene );

					// Delete Level1, as its not needed anymore
					AssetDatabase.DeleteAsset( "Assets/Map.unity" );
					AssetDatabase.Refresh();
				}

				//
				// Export Meta Data
				//
			}
		}

#endif
	}
}
