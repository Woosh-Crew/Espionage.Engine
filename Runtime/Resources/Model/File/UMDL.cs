using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine.Resources.Models
{
	[Title( "Unity Model File" ), File( Extension = "umdl" )]
	public partial class UMDL : AssetBundleFile { }

	#if UNITY_EDITOR

	public partial class UMDL : ICompiler<GameObject>
	{
		public void Compile( GameObject asset )
		{
			var exportPath = $"Exports/{ClassInfo.Group}/";
			var assetPath = AssetDatabase.GetAssetPath( asset );

			// Track how long exporting took
			using ( Debugging.Stopwatch( "Model Compiled", true ) )
			{
				try
				{
					// Find relative folder
					if ( Files.Pathing.InFolder( "Models", assetPath, "project://" ) )
					{
						var split = assetPath.Split( '/' );
						for ( var i = split.Length - 1; i >= 0; i-- )
						{
							if ( split[i] != "Models" )
							{
								continue;
							}

							exportPath += string.Join( '/', split[(i + 1)..^1] );
							break;
						}
					}

					Files.Pathing.Create( exportPath );

					var extension = ClassInfo.Components.Get<FileAttribute>().Extension;
					var builds = new[] { new AssetBundleBuild() { assetNames = new[] { assetPath }, assetBundleName = $"{Files.Pathing.Name( assetPath, false )}.{extension}" } };

					var bundle = BuildPipeline.BuildAssetBundles( exportPath, builds, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows );

					if ( bundle == null )
					{
						EditorUtility.DisplayDialog( "Model Failed to Compile", "Model asset bundle compile failed.", "Okay" );
						return;
					}

					Files.Delete( Files.Pathing.Absolute( exportPath ), "manifest", "" );
				}
				catch ( Exception e )
				{
					Debugging.Log.Exception( e );
				}
			}
		}
	}

	#endif
}
