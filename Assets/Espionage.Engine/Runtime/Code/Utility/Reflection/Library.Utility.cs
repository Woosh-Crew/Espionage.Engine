using System;
using System.Collections.Generic;
using System.Linq;
using Espionage.Engine;

public static class LibraryDatabaseExtensions
{
	//
	// Try Get
	//

	public static bool TryGet( this IDatabase<Library> database, string name, out Library library )
	{
		library = database.Get( name );
		return library is null;
	}

	public static bool TryGet( this IDatabase<Library> database, Type type, out Library library )
	{
		library = database.Get( type );
		return library is null;
	}

	public static bool TryGet( this IDatabase<Library> database, Guid id, out Library library )
	{
		library = database.Get( id );
		return library is null;
	}

	//
	// Exists
	//

	public static bool Exists( this IDatabase<Library> database, string name ) => database.Get( name ) is not null;
	public static bool Exists( this IDatabase<Library> database, Type type ) => database.Get( type ) is not null;
	public static bool Exists( this IDatabase<Library> database, Guid id ) => database.Get( id ) is not null;

	//
	// Get
	//

	public static Library Get( this IDatabase<Library> database, string name )
	{
		return database.All.FirstOrDefault( e => e.Name == name );
	}

	public static Library Get( this IDatabase<Library> database, Type type )
	{
		return database.All.FirstOrDefault( e => e.Owner == type );
	}

	public static Library Get( this IDatabase<Library> database, Guid id )
	{
		return database.All.FirstOrDefault( e => e.Id == id );
	}

	public static Library Get<T>( this IDatabase<Library> database ) where T : ILibrary
	{
		return database.All.FirstOrDefault( e => e.Owner == typeof( T ) );
	}

	//
	// Create
	//

	public static T Create<T>( this IDatabase<Library> database ) where T : class, ILibrary, new()
	{
		return Library.Construct( database.Get<T>() ) as T;
	}

	public static T Create<T>( this IDatabase<Library> database, string name, bool assertMissing = false ) where T : class, ILibrary, new()
	{
		if ( !database.TryGet( name, out var library ) )
		{
			if ( assertMissing )
				Debugging.Log.Error( $"Library doesnt contain [{name}], not creating ILibrary" );

			return null;
		}

		return Library.Construct( library ) as T;
	}

	public static T Create<T>( this IDatabase<Library> database, Guid id ) where T : class, ILibrary, new()
	{
		var library = database.Get( id );

		if ( id == default )
		{
			Debugging.Log.Error( "Invalid ID" );
			return null;
		}

		return Library.Construct( library ) as T;
	}
}
