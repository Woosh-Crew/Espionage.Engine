using System.IO;
using Espionage.Engine.Resources.Binders;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine.Resources.Formats
{
	[Title( "UI Toolkit File" ), File( Extension = "uitk" )]
	public sealed partial class UITK : UI.File
	{
		public override UI.Binder Binder => new AssetBundleUIBinder( Source.FullName );
	}

	#if UNITY_EDITOR

	public partial class UITK : ICompiler<VisualTreeAsset>
	{
		public void Compile( string asset )
		{
			var exportPath = $"Exports/{ClassInfo.Group}/";
			var extension = ClassInfo.Components.Get<FileAttribute>()?.Extension;

			if ( string.IsNullOrEmpty( extension ) )
			{
				Debugging.Log.Error( $"{ClassInfo.Title} doesn't have an extension. Not compiling" );
				return;
			}

			using ( Debugging.Stopwatch( $"{ClassInfo.Title} Compiled" ) )
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
