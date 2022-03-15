using Espionage.Engine.Resources.Binders;

namespace Espionage.Engine.Resources.Formats
{
	[Title( "Ravenfield Level File" ), File( Extension = "rfl" )]
	public sealed class RFL : Map.File
	{
		public override Map.Binder Binder => new AssetBundleMapProvider( Source.FullName );
	}
}
