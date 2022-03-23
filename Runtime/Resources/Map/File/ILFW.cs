using Espionage.Engine.Resources.Binders;

namespace Espionage.Engine.Resources.Formats
{
	[Title( "Intruder Level File" ), File( Extension = "ilfw" )]
	public sealed class ILFW : Map.File
	{
		public override Map.Binder Binder => new AssetBundleMapProvider( Source.FullName );
	}
}
