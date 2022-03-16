using System;
using System.IO;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	public sealed partial class Map
	{
		[Function( "map.load" ), Terminal]
		private static void LoadMap( string path )
		{
			var map = Find( path );
			map?.Load();
		}

		[Function( "map.current" ), Terminal]
		private static void LoadMap()
		{
			if ( Current == null )
			{
				Dev.Log.Info( "Nothing" );
				return;
			}

			if ( Current.Components.TryGet( out Meta meta ) )
			{
				Dev.Log.Info( $"Map [{Current.Identifier}], Title [{meta.Title}], Author [{meta.Author}]" );
				return;
			}

			Dev.Log.Info( $"Map [{Current.Identifier}]" );
		}
	}
}
