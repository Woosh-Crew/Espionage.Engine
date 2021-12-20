using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Espionage.Engine.Internal
{
	public struct LibraryAccessor
	{
		public IEnumerable<Library> Records => Library.GetAll();

		// TryGet
		public bool TryGet( string name, out Library library )
		{
			library = Get( name );
			return library is null;
		}

		public bool TryGet( Type type, out Library library )
		{
			library = Get( type );
			return library is null;
		}

		public bool TryGet( Guid id, out Library library )
		{
			library = Get( id );
			return library is null;
		}

		// Get
		public Library Get( string name ) => Records.FirstOrDefault( e => e.Name == name );
		public Library Get( Type type ) => Records.FirstOrDefault( e => e.Owner == type );
		public Library Get( Guid id ) => Records.FirstOrDefault( e => e.Id == id );
		public Library Get<T>() where T : ILibrary => Records.FirstOrDefault( e => e.Owner == typeof( T ) );
	}

	public struct LibraryCreator
	{
		public LibraryAccessor Accessor => Library.Accessor;

		public T Create<T>() where T : class, ILibrary, new()
		{
			return Construct( Accessor.Get<T>() ) as T;
		}

		public T Create<T>( string name, bool assertMissing = false ) where T : class, ILibrary, new()
		{
			if ( !Accessor.TryGet( name, out var library ) )
			{
				if ( assertMissing )
					Debug.LogError( $"Library doesnt contain [{name}], not creating ILibrary" );

				return null;
			}

			return Construct( library ) as T;
		}

		public T Create<T>( Guid id ) where T : class, ILibrary, new()
		{
			var library = Accessor.Get( id );

			if ( id == default )
			{
				Debug.LogError( "Invalid ID" );
				return null;
			}

			return Construct( library ) as T;
		}

		/// <summary> Constructs ILibrary, if it it has a custom constructor
		/// itll use that to create the ILibrary </summary>
		public ILibrary Construct( Library library )
		{
			if ( library is null )
			{
				Debug.LogError( "Can't construct, Library is null" );
				return null;
			}

			if ( library.Constructor is not null )
				return library.Constructor.Invoke( null, new object[] { library.Owner } ) as ILibrary;

			return Activator.CreateInstance( library.Owner ) as ILibrary;
		}
	}
}
