using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using Espionage.Engine.IO;
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
			Pathing exportPath = $"Exports/{ClassInfo.Group}/";
			Pathing assetPath = AssetDatabase.GetAssetPath( asset );

			// Track how long exporting took
			using ( Debugging.Stopwatch( "Model Compiled", true ) )
			{
				try
				{
					// Find relative folder
					if ( assetPath.InFolder( "Models", "project://" ) )
					{
						var split = assetPath.Output.Split( '/' );
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

					exportPath.Create();

					var extension = ClassInfo.Components.Get<FileAttribute>().Extension;
					var builds = new[] { new AssetBundleBuild() { assetNames = new string[] { assetPath }, assetBundleName = $"{assetPath.Name( false )}.{extension}" } };

					var bundle = BuildPipeline.BuildAssetBundles( exportPath, builds, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows );

					if ( bundle == null )
					{
						EditorUtility.DisplayDialog( "Model Failed to Compile", "Model asset bundle compile failed.", "Okay" );
						return;
					}

					Files.Delete( exportPath.Absolute(), "manifest", "" );
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
