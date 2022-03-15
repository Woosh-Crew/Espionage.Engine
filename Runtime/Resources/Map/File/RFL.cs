using System.IO;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources.Formats
{
	[Group( "Maps" ), Title( "Ravenfield Level File" ), File( Extension = "rfl" )]
	public class RFL : IFile<Map, Scene>, IAsset
	{
		public Library ClassInfo { get; } = Library.Database[typeof( RFL )];

		// Resource

		public FileInfo File { get; set; }
		public void Load( FileStream fileStream ) { }

		// Provider

		public Resource.IProvider<Map, Scene> Provider => new AssetBundleMapProvider( File );
	}
}
