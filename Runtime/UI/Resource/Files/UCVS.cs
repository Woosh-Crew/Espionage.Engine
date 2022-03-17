using System.IO;
using Espionage.Engine.Resources.Binders;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine.Resources.Formats
{
	[Title( "Unity Canvas File" ), File( Extension = "ucvs" )]
	public sealed partial class UCVS : UI.File
	{
		public override UI.Binder Binder => new AssetBundleCanvasBinder( Source.FullName );
	}

	#if UNITY_EDITOR

	public partial class UCVS : ICompiler<CanvasAsset>
	{
		public void Compile( string asset )
		{
			var exportPath = $"Exports/{ClassInfo.Group}/";
			var extension = ClassInfo.Components.Get<FileAttribute>()?.Extension;

			if ( string.IsNullOrEmpty( extension ) )
			{
				Dev.Log.Error( $"{ClassInfo.Title} doesn't have an extension. Not compiling" );
				return;
			}

			using ( Dev.Stopwatch( $"{ClassInfo.Title} Compiled" ) )
			{
				try
				{
					if ( !Directory.Exists( Path.GetFullPath( exportPath ) ) )
					{
						Directory.CreateDirectory( Path.GetFullPath( exportPath ) );
					}

					var builds = new[] { new AssetBundleBuild() { assetNames = new[] { asset }, assetBundleName = $"{Path.GetFileNameWithoutExtension( asset )}.{extension}" } };
					var bundle = BuildPipeline.BuildAssetBundles( exportPath, builds, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows );

					if ( bundle is null )
					{
						EditorUtility.DisplayDialog( "ERROR", $"Asset bundle compile failed.", "Okay" );
						return;
					}

					Files.Delete( $"assets://{ClassInfo.Group}", "manifest", "" );
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
