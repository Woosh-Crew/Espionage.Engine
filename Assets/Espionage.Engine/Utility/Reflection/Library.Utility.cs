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

	public static bool TryGet<T>( this IDatabase<Library> database, out Library library ) where T : ILibrary
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

	public static bool Exists<T>( this IDatabase<Library> database ) where T : ILibrary => database.Get<T>() is not null;
	public static bool Exists( this IDatabase<Library> database, string name ) => database.Get( name ) is not null;
	public static bool Exists( this IDatabase<Library> database, Type type ) => database.Get( type ) is not null;
	public static bool Exists( this IDatabase<Library> database, Guid id ) => database.Get( id ) is not null;

	//
	// Get
	//

	public static Library Get<T>( this IDatabase<Library> database ) where T : ILibrary
	{
		return database.All.FirstOrDefault( e => e.Class == typeof( T ) );
	}

	public static Library Get( this IDatabase<Library> database, string name )
	{
		return database.All.FirstOrDefault( e => e.name == name );
	}

	public static Library Get( this IDatabase<Library> database, Type type )
	{
		return database.All.FirstOrDefault( e => e.Class == type );
	}

	public static Library Get( this IDatabase<Library> database, Guid id )
	{
		return database.All.FirstOrDefault( e => e.Id == id );
	}

	//
	// Get All
	//

	public static IEnumerable<Library> GetAll<T>( this IDatabase<Library> database ) where T : ILibrary
	{
		return !database.TryGet<T>( out var item ) ? null : database.All.Where( e => e.Class.IsSubclassOf( item.Class ) );
	}

	public static IEnumerable<Library> GetAll( this IDatabase<Library> database, Type type )
	{
		return !database.TryGet( type, out var item ) ? null : database.All.Where( e => e.Class.IsSubclassOf( item.Class ) );
	}

	public static IEnumerable<Library> GetAll( this IDatabase<Library> database, string name )
	{
		return !database.TryGet( name, out var item ) ? null : database.All.Where( e => e.Class.IsSubclassOf( item.Class ) );
	}

	public static IEnumerable<Library> GetAll( this IDatabase<Library> database, Guid id )
	{
		return !database.TryGet( id, out var item ) ? null : database.All.Where( e => e.Class.IsSubclassOf( item.Class ) );
	}

	//
	// Create
	//

	public static T Create<T>( this IDatabase<Library> database ) where T : class, ILibrary, new()
	{
		return Library.Construct( database.Get<T>() ) as T;
	}

	public static ILibrary Create( this IDatabase<Library> database, Type type )
	{
		return Library.Construct( database.Get( type ) );
	}

	public static T Create<T>( this IDatabase<Library> database, Type type ) where T : class, ILibrary
	{
		return database.Create( type ) as T;
	}

	public static ILibrary Create( this IDatabase<Library> database, string name, bool assertMissing = false )
	{
		if ( database.TryGet( name, out var library ) )
			return Library.Construct( library );

		if ( assertMissing )
			Debugging.Log.Error( $"Library doesnt contain [{name}], not creating ILibrary" );

		return null;

	}

	public static T Create<T>( this IDatabase<Library> database, string name, bool assertMissing = false ) where T : class, ILibrary, new()
	{
		return database.Create( name, assertMissing ) as T;
	}

	public static ILibrary Create( this IDatabase<Library> database, Guid id )
	{
		var library = database.Get( id );

		if ( id != default )
			return Library.Construct( library );

		Debugging.Log.Error( "Invalid ID" );
		return null;

	}

	public static T Create<T>( this IDatabase<Library> database, Guid id ) where T : class, ILibrary, new()
	{
		return database.Create( id ) as T;
	}

	//
	// Replace
	//

	public static void Replace<T>( this IDatabase<Library> database, Library newLibrary ) where T : ILibrary
	{
		if ( database.TryGet<T>( out var item ) )
		{
			database.Replace( item, newLibrary );
		}
		else
		{
			Debugging.Log.Warning( $"Couldn't not find {typeof( T ).FullName} in Library database" );
		}
	}

	public static void Replace( this IDatabase<Library> database, string name, Library newLibrary )
	{
		if ( database.TryGet( name, out var item ) )
		{
			database.Replace( item, newLibrary );
		}
		else
		{
			Debugging.Log.Warning( $"Couldn't not find {name} in Library database" );
		}
	}

	public static void Replace( this IDatabase<Library> database, Guid id, Library newLibrary )
	{
		if ( database.TryGet( id, out var item ) )
		{
			database.Replace( item, newLibrary );
		}
		else
		{
			Debugging.Log.Warning( $"Couldn't not find {id} in Library database" );
		}
	}

	public static void Replace( this IDatabase<Library> database, Type type, Library newLibrary )
	{
		if ( database.TryGet( type, out var item ) )
		{
			database.Replace( item, newLibrary );
		}
		else
		{
			Debugging.Log.Warning( $"Couldn't not find {type.FullName} in Library database" );
		}
	}
}

//
// IDatabase<Library.Component>
//
public static class LibraryComponentDatabaseExtensions
{
	public static T Get<T>( this IDatabase<Library.IComponent> database ) where T : Library.IComponent
	{
		return (T)database.All.FirstOrDefault( e => e is T );
	}

	public static IEnumerable<T> GetAll<T>( this IDatabase<Library.IComponent> database ) where T : Library.IComponent
	{
		return database.All.Select( e => (T)e );
	}

	public static bool TryGet<T>( this IDatabase<Library.IComponent> database, out T component ) where T : Library.IComponent
	{
		component = database.Get<T>();
		return component is not null;
	}

	public static bool Exists<T>( this IDatabase<Library.IComponent> database ) where T : Library.IComponent
	{
		return database.Get<T>() is not null;
	}

}
