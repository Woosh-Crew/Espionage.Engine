using System.IO;

namespace Espionage.Engine.Resources.Formats
{
	[Title( "Ravenfield Level File" ), File( Extension = "rfl" )]
	public sealed class RFL : IFile<Map>
	{
		public Library ClassInfo { get; } = Library.Database[typeof( RFL )];

		public FileInfo File { get; set; }
		public void Load( FileStream fileStream ) { }

		public IBinder<Map> Binder => new AssetBundleMapProvider( File );
	}
}
