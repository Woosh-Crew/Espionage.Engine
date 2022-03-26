using System;
using System.Collections.Generic;
using Espionage.Engine.Components;

namespace Espionage.Engine.Resources
{
	public sealed partial class Map
	{
		public static Factory Setup { get; } = new();

		public readonly struct Factory { }

		public readonly struct Builder
		{
			private readonly string _identifier;
			private readonly Binder _provider;

			private readonly Dictionary<Type, IComponent<Map>> _components;

			internal Builder( string identifier )
			{
				_identifier = identifier;
				_components = new();
				_provider = null;
			}

			internal Builder( Binder provider, string id )
			{
				_identifier = id;
				_provider = provider;
				_components = new();
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
				var map = _provider == null ? Find( _identifier ) : new( _provider, _identifier );

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
