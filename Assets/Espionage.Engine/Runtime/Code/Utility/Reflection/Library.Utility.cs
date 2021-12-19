using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
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

		public T Create<T>() where T : Entity, new()
		{
			var newEntity = new GameObject( Accessor.Get<T>().Name ).AddComponent<T>();
			newEntity.Spawn();

			if ( NetworkServer.active )
				NetworkServer.Spawn( newEntity.gameObject, newEntity.ClassInfo.Id );

			return newEntity;
		}

		public Entity Create( string name, bool assertMissing = false )
		{
			if ( !Accessor.TryGet( name, out var library ) )
			{
				if ( assertMissing )
					Debug.LogError( $"Library doesnt contain [{name}], not creating ILibrary" );
				return null;
			}

			var newEntity = new GameObject( library.Name ).AddComponent( library.Owner ) as Entity;
			newEntity.Spawn();

			if ( NetworkServer.active )
				NetworkServer.Spawn( newEntity.gameObject, library.Id );

			return newEntity;
		}

		public Entity Create( Guid id )
		{
			var library = Accessor.Get( id );

			if ( id == default )
				throw new Exception( "Invalid Id" );

			var newObject = new GameObject( library.Owner.FullName ).AddComponent( library.Owner );

			if ( NetworkServer.active )
				NetworkServer.Spawn( newObject.gameObject, library.Id );

			return newObject as Entity;
		}
	}
}
