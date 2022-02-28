using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine.Resources
{
	/// <summary>
	/// An asset that can be compiled for use at Runtime.
	/// </summary>
	[Group( "Resources" )]
	public abstract class Asset : ScriptableObject, ILibrary, IAsset
	{
		public Library ClassInfo { get; private set; }

		private void OnEnable()
		{
			ClassInfo = Library.Register( this );
		}

		private void OnDisable()
		{
			Library.Unregister( this );
		}

		public virtual bool CanCompile()
		{
			return true;
		}

	#if UNITY_EDITOR

		public virtual void Compile( params BuildTarget[] targets )
		{
			var exportPath = $"Exports/{ClassInfo.Group}/{name}/";
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

					var builds = new[]
					{
						new AssetBundleBuild()
						{
							assetNames = new[] { AssetDatabase.GetAssetPath( this ) },
							assetBundleName = $"{name}.{extension}"
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
		}

	#endif
	}
}
