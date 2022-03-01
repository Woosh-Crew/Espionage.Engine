using System.IO;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	[Group( "Files" ), Title( "UMAP File" ), File( Extension = "umap" )]
	public sealed class UMAP : IFile<Map, Scene>
	{
		public Library ClassInfo { get; }

		public UMAP() { ClassInfo = Library.Register( this ); }

		~UMAP() { Library.Unregister( this ); }

		// Resource

		public FileInfo File { get; set; }
		public void Load( FileStream fileStream ) { }

		// Provider

		public Resource.IProvider<Map, Scene> Provider()
		{
			// This file is basically an Asset Bundle
			return new AssetBundleMapProvider( File );
		}
	}
}
