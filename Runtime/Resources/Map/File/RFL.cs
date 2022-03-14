using System.IO;

namespace Espionage.Engine.Resources.Formats
{
	[Group( "Maps" ), Title( "Ravenfield Level File" ), File( Extension = "rfl" )]
	public class RFL : IFile<Map>, IAsset
	{
		public Library ClassInfo { get; } = Library.Database[typeof( RFL )];

		// Resource

		public FileInfo File { get; set; }
		public void Load( FileStream fileStream ) { }

		// Provider

		public Resource.IProvider<Map> Provider => new AssetBundleMapProvider( File );
	}
}
