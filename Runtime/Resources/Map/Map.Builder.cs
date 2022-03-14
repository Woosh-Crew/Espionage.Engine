using System.Collections.Generic;
using Espionage.Engine.Components;

namespace Espionage.Engine.Resources
{
	public sealed partial class Map
	{
		/// <summary>
		/// Sets up a builder for the map using a path, Allowing you 
		/// to easily control its data through a build setup.
		/// </summary>
		public static Builder Setup( string path )
		{
			path = Files.Pathing.Absolute( path );

			// Use the Database Map if we have it
			if ( Exists( path ) )
			{
				Debugging.Log.Info( $"Map [{path}], already exists" );
				return default;
			}

			return new( path );
		}

		/// <summary>
		/// Sets up a builder for the map using a provider, Allowing you 
		/// to easily control its data through a build setup.
		/// </summary>
		public static Builder Setup( Resource.IProvider<Map> provider )
		{
			return new( provider );
		}

		/// <summary>
		/// Sets up a builder for the map using a provider, Allowing you 
		/// to easily control its data through a build setup.
		/// </summary>
		public static Builder Setup( int buildIndex )
		{
			return new( new BuildIndexMapProvider( buildIndex ) );
		}
		
		public readonly struct Builder
		{
			private readonly string _path;
			private readonly Resource.IProvider<Map> _provider;

			private readonly List<IComponent<Map>> _components;

			internal Builder( string path )
			{
				_path = path;
				_components = new();

				_provider = null;
			}

			internal Builder( Resource.IProvider<Map> provider )
			{
				_provider = provider;
				_components = new();

				_path = null;
			}

			public Builder With<T>( T component ) where T : IComponent<Map>
			{
				_components.Add( component );
				return this;
			}

			public Builder Origin( string name )
			{
				_components.Add( new Origin() { Name = name } );
				return this;
			}

			public Map Build()
			{
				var map = _provider == null ? Find( _path ) : new( _provider );

				foreach ( var component in _components )
				{
					map.Components.Add( component );
				}

				return map;
			}
		}
	}
}
