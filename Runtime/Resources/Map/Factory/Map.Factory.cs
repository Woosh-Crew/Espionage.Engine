using Espionage.Engine.Resources.Maps;

namespace Espionage.Engine.Resources
{
	public static class MapFactoryExtensions
	{
		/// <summary>
		/// Sets up a builder for the map using a path, Allowing you 
		/// to easily control its data through a build setup.
		/// </summary>
		public static Map.Builder? Path( this Map.Factory factory, string path )
		{
			path = Files.Pathing.Absolute( path );

			if ( !Files.Pathing.Exists( path ) )
			{
				Debugging.Log.Info( $"Path [{path}], doesn't exist" );
				return null;
			}

			// Use the Database Map if we have it
			if ( Map.Exists( path ) )
			{
				Debugging.Log.Info( $"Map [{path}], already exists" );
				return null;
			}

			return new( path );
		}

		/// <summary>
		/// Sets up a builder for the map using a provider, Allowing you 
		/// to easily control its data through a build setup.
		/// </summary>
		public static Map.Builder Binder( this Map.Factory factory, Map.Binder binder, string id )
		{
			return new( binder, id );
		}

		/// <summary>
		/// Sets up a builder for the map using a provider, Allowing you 
		/// to easily control its data through a build setup.
		/// </summary>
		public static Map.Builder Index( this Map.Factory factory, int buildIndex )
		{
			return new( new BuildIndexMapProvider( buildIndex ), $"id:{buildIndex}" );
		}
	}
}
