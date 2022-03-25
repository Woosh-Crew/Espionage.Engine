using System;
using System.Collections.Generic;
using Espionage.Engine.Components;
using Espionage.Engine.Resources.Binders;

namespace Espionage.Engine.Resources
{
	public sealed partial class Map
	{
		public static Factory Setup { get; } = new();

		public readonly struct Factory
		{
			/// <summary>
			/// Sets up a builder for the map using a path, Allowing you 
			/// to easily control its data through a build setup.
			/// </summary>
			public Builder Path( string path )
			{
				path = Files.Pathing.Absolute( path );

				if ( !Files.Pathing.Exists( path ) )
				{
					Dev.Log.Info( $"Path [{path}], doesn't exist" );
					return default;
				}

				// Use the Database Map if we have it
				if ( Exists( path ) )
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
			public Builder Binder( Binder binder )
			{
				return new( binder );
			}

			/// <summary>
			/// Sets up a builder for the map using a provider, Allowing you 
			/// to easily control its data through a build setup.
			/// </summary>
			public Builder Index( int buildIndex )
			{
				return new( new BuildIndexMapProvider( buildIndex ) );
			}
		}

		public readonly struct Builder
		{
			private readonly string _path;
			private readonly Binder _provider;

			private readonly Dictionary<Type, IComponent<Map>> _components;

			internal Builder( string path )
			{
				_path = path;
				_components = new();

				_provider = null;
			}

			internal Builder( Binder provider )
			{
				_provider = provider;
				_components = new();

				_path = null;
			}

			public Builder With<T>( T component ) where T : IComponent<Map>
			{
				if ( _components.ContainsKey( typeof( T ) ) )
				{
					Dev.Log.Warning( $"Builder already contains component, {typeof( T ).FullName}" );
					return this;
				}

				_components.Add( typeof( T ), component );
				return this;
			}

			//
			// Defaults
			//

			public Builder Meta( string title, string description = null, string author = null )
			{
				if ( _components.ContainsKey( typeof( Meta ) ) )
				{
					var meta = (Meta)_components[typeof( Meta )];
					meta.Title = title;
					meta.Description = description;
					meta.Author = author;
					return this;
				}

				// Only Apply if the database doesnt have it yet.
				With( new Meta()
				{
					Title = title,
					Description = description,
					Author = author
				} );
				return this;
			}


			public Builder Origin( string name )
			{
				if ( _components.ContainsKey( typeof( Origin ) ) )
				{
					return this;
				}

				// Only Apply if the database doesnt have it yet.
				With( new Origin() { Name = name } );
				return this;
			}

			public Builder Thumbnail( string path )
			{
				if ( _components.ContainsKey( typeof( Thumbnail ) ) )
				{
					((Thumbnail)_components[typeof( Thumbnail )]).Path = path;
					return this;
				}

				// Only Apply if the database doesnt have it yet.
				With( new Thumbnail( path ) );
				return this;
			}

			//
			// Builder
			//

			public Map Build()
			{
				var map = _provider == null ? Find( _path ) : new( _provider );

				foreach ( var (key, component) in _components )
				{
					if ( map.Components.Has( key ) )
					{
						map.Components.Replace( map.Components.Get( key ), component );
						continue;
					}

					map.Components.Add( component );
				}

				return map;
			}
		}
	}
}
