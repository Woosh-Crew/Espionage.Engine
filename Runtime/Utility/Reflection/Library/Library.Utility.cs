using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Espionage.Engine;

//
// IDatabase<Library>
//

public static class LibraryDatabaseExtensions
{
	//
	// Try Get
	//

	public static bool TryGet<T>( this IDatabase<Library> database, out Library library ) where T : class
	{
		library = database.Get<T>();
		return library is not null;
	}

	public static bool TryGet( this IDatabase<Library> database, string name, out Library library )
	{
		library = database.Get( name );
		return library is not null;
	}

	public static bool TryGet( this IDatabase<Library> database, Type type, out Library library )
	{
		library = database.Get( type );
		return library is not null;
	}

	public static bool TryGet( this IDatabase<Library> database, Guid id, out Library library )
	{
		library = database.Get( id );
		return library is not null;
	}

	//
	// Exists
	//

	public static bool Exists<T>( this IDatabase<Library> database ) where T : class
	{
		return database.Get<T>() is not null;
	}

	public static bool Exists( this IDatabase<Library> database, string name )
	{
		return database.Get( name ) is not null;
	}

	public static bool Exists( this IDatabase<Library> database, Type type )
	{
		return database.Get( type ) is not null;
	}

	public static bool Exists( this IDatabase<Library> database, Guid id )
	{
		return database.Get( id ) is not null;
	}

	//
	// Get
	//

	public static Library Get<T>( this IDatabase<Library> database ) where T : class
	{
		return database.FirstOrDefault( e => e.Info == typeof( T ) );
	}

	public static Library Get( this IDatabase<Library> database, string name )
	{
		return database.FirstOrDefault( e => e.Name == name );
	}

	public static Library Get( this IDatabase<Library> database, Type type )
	{
		return database.FirstOrDefault( e => e.Info == type );
	}

	public static Library Get( this IDatabase<Library> database, Guid id )
	{
		return database.FirstOrDefault( e => e.Id == id );
	}

	//
	// Get All
	//

	public static Library Find( this IDatabase<Library> database, Type type )
	{
		return type.IsInterface
			? database.FirstOrDefault( e => e.Info.HasInterface( type ) && !e.Info.IsAbstract )
			: database.FirstOrDefault( e =>
				(type == e.Info || e.Info.IsSubclassOf( type )) && !e.Info.IsAbstract );
	}

	public static Library Find( this IDatabase<Library> database, Type type, Func<Library, bool> search )
	{
		return type.IsInterface
			? database.FirstOrDefault( e =>
				e.Info.HasInterface( type ) && !e.Info.IsAbstract && search.Invoke( e ) )
			: database.FirstOrDefault( e =>
				(type == e.Info || e.Info.IsSubclassOf( type )) && !e.Info.IsAbstract && search.Invoke( e ) );
	}

	public static Library Find<T>( this IDatabase<Library> database ) where T : class
	{
		return database.Find( typeof( T ) );
	}

	public static Library Find<T>( this IDatabase<Library> database, Func<Library, bool> search ) where T : class
	{
		return database.Find( typeof( T ), search );
	}

	public static IEnumerable<Library> GetAll<T>( this IDatabase<Library> database ) where T : class
	{
		var type = typeof( T );

		if ( type.IsInterface )
		{
			return database.Where( e => e.Info.HasInterface<T>() );
		}

		return !database.TryGet<T>( out var item ) ? null : database.Where( e => e.Info.IsSubclassOf( item.Info ) );
	}

	public static IEnumerable<Library> GetAll( this IDatabase<Library> database, Type type )
	{
		return !database.TryGet( type, out var item ) ? null : database.Where( e => e.Info.IsSubclassOf( item.Info ) );
	}

	public static IEnumerable<Library> GetAll( this IDatabase<Library> database, string name )
	{
		return !database.TryGet( name, out var item ) ? null : database.Where( e => e.Info.IsSubclassOf( item.Info ) );
	}

	public static IEnumerable<Library> GetAll( this IDatabase<Library> database, Guid id )
	{
		return !database.TryGet( id, out var item ) ? null : database.Where( e => e.Info.IsSubclassOf( item.Info ) );
	}

	//
	// Create
	//

	public static T Create<T>( this IDatabase<Library> database ) where T : class, new()
	{
		return Library.Create( database.Get<T>() ) as T;
	}

	public static T Create<T>( this IDatabase<Library> database, Library library ) where T : class, ILibrary
	{
		return Library.Create( library ) as T;
	}

	public static ILibrary Create( this IDatabase<Library> database, Type type )
	{
		return Library.Create( database.Get( type ) );
	}

	public static T Create<T>( this IDatabase<Library> database, Type type ) where T : class, ILibrary
	{
		return database.Create( type ) as T;
	}

	public static ILibrary Create( this IDatabase<Library> database, string name, bool assertMissing = false )
	{
		if ( database.TryGet( name, out var library ) )
		{
			return Library.Create( library );
		}

		if ( assertMissing )
		{
			Dev.Log.Error( $"Library doesnt contain [{name}], not creating ILibrary" );
		}

		return null;
	}

	public static T Create<T>( this IDatabase<Library> database, string name, bool assertMissing = false ) where T : class, ILibrary
	{
		return database.Create( name, assertMissing ) as T;
	}

	public static ILibrary Create( this IDatabase<Library> database, Guid id )
	{
		var library = database.Get( id );

		if ( id != default )
		{
			return Library.Create( library );
		}

		Dev.Log.Error( "Invalid ID" );
		return null;
	}

	public static T Create<T>( this IDatabase<Library> database, Guid id ) where T : class, new()
	{
		return database.Create( id ) as T;
	}
}
