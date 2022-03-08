using System.IO;
using Espionage.Engine.Resources.Provider;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine.Resources
{
	[Group( "Models" ), Title( "UMDL File" ), File( Extension = "umdl" )]
	public sealed class UMDL : IFile<Model, GameObject>, IAsset
	{
		public Library ClassInfo { get; }


		public UMDL() { ClassInfo = Library.Register( this ); }

		~UMDL() { Library.Unregister( this ); }

		// Resource

		public FileInfo File { get; set; }
		public void Load( FileStream fileStream ) { }

		// Provider

		public Resource.IProvider<Model, GameObject> Provider()
		{
			return new AssetBundleModelProvider( File );
		}

		// Compiler

	#if UNITY_EDITOR

		public static void Compile( string assetPath ) { }

	#endif
	}
}
