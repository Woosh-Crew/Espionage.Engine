using System.IO;
using UnityEditor;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEngine;
#endif

namespace Espionage.Engine.Resources
{
	public abstract class Asset<T> : ScriptableObject, ILibrary where T : Object
	{
		public Library ClassInfo { get; private set; }

		private void OnEnable()
		{
			ClassInfo = Library.Database[GetType()];
		}

		//
		// Asset
		//

		public T asset;

		public virtual void Compile()
		{
#if UNITY_EDITOR
			var exportPath = $"Exports/{ClassInfo.Group}/{name}/";

			using ( Debugging.Stopwatch( $"{ClassInfo.Title} Compiled" ) )
			{
				try
				{
					if ( !Directory.Exists( Path.GetFullPath( exportPath ) ) )
					{
						Directory.CreateDirectory( Path.GetFullPath( exportPath ) );
					}

					var builds = new[]
					{
						new AssetBundleBuild()
						{
							assetNames = new[] { AssetDatabase.GetAssetPath( this ) },
							assetBundleName = $"{name}.ast"
						}
					};

					var bundle = BuildPipeline.BuildAssetBundles( exportPath, builds, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows );

					if ( bundle is null )
					{
						EditorUtility.DisplayDialog( "ERROR", $"Map asset bundle compile failed.", "Okay" );
						return;
					}
				}
				finally
				{
					AssetDatabase.Refresh();
				}
			}
#else
			Debugging.Log.Error("You can only compile while in the editor.");
#endif
		}
	}
}
