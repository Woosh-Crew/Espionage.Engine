using System;
using System.Collections.Generic;
using Espionage.Engine.Components;
using Espionage.Engine.Resources.Binders;

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
		public static Builder Setup( Binder provider )
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
					Debugging.Log.Warning( $"Builder already contains component, {typeof( T ).FullName}" );
					return this;
				}

				_components.Add( typeof( T ), component );
				return this;
			}

			//
			// Defaults
			//

			public Builder Title( string title )
			{
				if ( _components.ContainsKey( typeof( Meta ) ) )
				{
					((Meta)_components[typeof( Meta )]).Title = title;
					return this;
				}

				// Only Apply if the database doesnt have it yet.
				With( new Meta() { Title = title } );
				return this;
			}

			public Builder Description( string desc )
			{
				if ( _components.ContainsKey( typeof( Meta ) ) )
				{
					((Meta)_components[typeof( Meta )]).Description = desc;
					return this;
				}

				// Only Apply if the database doesnt have it yet.
				With( new Meta() { Description = desc } );
				return this;
			}

			public Builder Author( string author )
			{
				if ( _components.ContainsKey( typeof( Meta ) ) )
				{
					((Meta)_components[typeof( Meta )]).Author = author;
					return this;
				}

				// Only Apply if the database doesnt have it yet.
				With( new Meta() { Author = author } );
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

				foreach ( var component in _components.Values )
				{
					map.Components.Add( component );
				}

				return map;
			}
		}
	}
}
