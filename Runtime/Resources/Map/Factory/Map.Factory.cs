using Espionage.Engine.Resources.Binders;

namespace Espionage.Engine.Resources
{
	public static class MapFactoryExtensions
	{
		/// <summary>
		/// Sets up a builder for the map using a path, Allowing you 
		/// to easily control its data through a build setup.
		/// </summary>
		public static Map.Builder Path( this Map.Factory factory, string path )
		{
			path = Files.Pathing.Absolute( path );

			if ( !Files.Pathing.Exists( path ) )
			{
				Dev.Log.Info( $"Path [{path}], doesn't exist" );
				return default;
			}

			// Use the Database Map if we have it
			if ( Map.Exists( path ) )
			{
				Dev.Log.Info( $"Map [{path}], already exists" );
				return default;
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
