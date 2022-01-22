namespace Espionage.Engine.Resources
{
	public partial class Map
	{
		[Debugging.Cmd("map")]
		private static void CmdGetMap( )
		{
			Debugging.Log.Info( "This would print the current map" );
		}
		
		[Debugging.Cmd( "map.load" )]
		private static Map CmdLoadFromPath( string path )
		{
			var map = new Map( path );
			map.Load();
			return map;
		}
	}
}
