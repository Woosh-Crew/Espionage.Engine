using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Espionage.Engine.IO;

namespace Espionage.Engine.Resources
{
	public sealed class Assets : Module
	{
		protected override void OnReady()
		{
			foreach ( var pathing in Library.Database.GetAll<IAsset>().Select( e => e.Components.Get<PathAttribute>() ) )
			{
				foreach ( var file in Files.Pathing( $"{pathing.ShortHand}://" ).All() )
				{
					Registered.Fill( file.Virtual().Normalise() );
				}
			}
		}

		protected override void OnUpdate()
		{
			Sweep();
		}

		protected override void OnShutdown()
		{
			foreach ( var resource in Registered )
			{
				resource.Unload( true );
			}
		}

		// Sweep

		private RealTimeSince _timeSinceSweep;
		private const int _timeBetweenSweeps = 60;

		private void Sweep()
		{
			if ( !(_timeSinceSweep > _timeBetweenSweeps) )
			{
				return;
			}

			_timeSinceSweep = 0;

			foreach ( var resource in Registered )
			{
				if ( resource.Instances?.Count == 0 && !resource.Persistant )
				{
					Debugging.Log.Info( $"No Instances of [{resource.Path}], Unloading" );
					resource.Unload( false );
				}
			}
		}

		// API

		public static T Load<T>( Pathing path, bool persistant = false ) where T : class, IAsset, new()
		{
			Library library = typeof( T );

			// Apply shorthand, if path doesn't have one
			if ( !path.IsValid() && library.Components.TryGet<PathAttribute>( out var attribute ) )
			{
				path = $"{attribute.ShortHand}://" + path;
			}

			var resource = Find( path.Virtual().Normalise() );
			return resource != null ? resource.Load<T>() : Fallback<T>();
		}

		public static T Fallback<T>() where T : class, IAsset, new()
		{
			Library library = typeof( T );

			// Load default resource, if its not there
			if ( !library.Components.TryGet( out FileAttribute files ) || files.Fallback.IsEmpty() )
			{
				return null;
			}

			Debugging.Log.Error( $"Loading fallback for [{library.Title}]" );

			Pathing fallback = files.Fallback;
			fallback = fallback.Virtual().Normalise();

			return !fallback.Exists() ? null : Load<T>( fallback, true );
		}

		public static Resource Find( Pathing path )
		{
			path = path.Virtual().Normalise();
			return Registered[path] != null ? Registered[path] : (path.Exists() ? Registered.Fill( path ) : null);
		}

		public static Resource Find( int hash )
		{
			return Registered[hash];
		}

		// Registry

		public static Registry Registered { get; } = new();

		public class Registry : IEnumerable<Resource>
		{
			private readonly SortedList<int, Resource> _storage = new();

			public Resource this[ int key ] => _storage.ContainsKey( key ) ? _storage[key] : null;

			public Resource this[ Pathing key ]
			{
				get
				{
					var hash = key.Hash();
					return _storage.ContainsKey( hash ) ? _storage[hash] : null;
				}
			}

			// Enumerator

			public IEnumerator<Resource> GetEnumerator()
			{
				return _storage.Values.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			// API

			public Resource Fill( Pathing path )
			{
				var hash = path.Virtual().Hash();

				if ( Registered[hash] != null )
				{
					return Registered[hash];
				}

				var instance = new Resource( path );

				_storage.Add( instance.Identifier, instance );
				return instance;
			}
		}
	}
}
