using System;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Espionage.Engine.Resources.Maps
{
	[Title( "Unity Map File" ), File( Extension = "umap" )]
	public sealed partial class UMAP : AssetBundleFile { }

	#if UNITY_EDITOR

	public partial class UMAP : ICompiler<SceneAsset>, ITester<SceneAsset>
	{
		public string Test( string asset )
		{
			return $"+map \"{Files.Pathing( $"maps://{Files.Pathing( asset ).Name( false )}.umap" ).Absolute()}\"";
		}

		public void Compile( SceneAsset asset )
		{
			var scenePath = AssetDatabase.GetAssetPath( asset );

			if ( SceneManager.GetActiveScene().path == scenePath && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() )
			{
				Debugging.Log.Warning( "Not compiling, User didn't want to save." );
				return;
			}

			var exportPath = $"Exports/{ClassInfo.Group}/";

			using ( Debugging.Stopwatch( $"Map [{asset.name}] Compiled", true ) )
			{
				try
				{
					Files.Pathing( exportPath ).Absolute().Create();

					var extension = ClassInfo.Components.Get<FileAttribute>().Extension;
					var bundle = BuildPipeline.BuildAssetBundles( exportPath, new[] { new AssetBundleBuild() { assetNames = new[] { scenePath }, assetBundleName = $"{asset.name}.{extension}" } },
						BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows );

					if ( bundle == null )
					{
						EditorUtility.DisplayDialog( "Map Failed to Compile", "Map asset bundle compile failed.", "Okay" );
						return;
					}

					Files.Delete( $"assets://{ClassInfo.Group}", "manifest", "" );
				}
				catch ( Exception e )
				{
					Debugging.Log.Exception( e );
				}
				finally
				{
					AssetDatabase.Refresh();
				}
			}
		}
	}

	#endif
}
