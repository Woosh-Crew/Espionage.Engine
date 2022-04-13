using System;
using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine
{
	public static class LibraryDatabaseExtensions
	{
		public static T Create<T>( this Libraries database ) where T : class, new()
		{
			return Library.Create( typeof( T ) ) as T;
		}

		public static Library Get<T>( this Libraries database ) where T : class
		{
			return Library.Database[typeof( T )];
		}

		//
		// Get All
		//

		public static Library Find( this Libraries database, Type type )
		{
			return type.IsInterface
				? database.FirstOrDefault( e => e.Info.HasInterface( type ) && !e.Info.IsAbstract )
				: database.FirstOrDefault( e =>
					(type == e.Info || e.Info.IsSubclassOf( type )) && !e.Info.IsAbstract );
		}

		public static Library Find( this Libraries database, Type type, Func<Library, bool> search )
		{
			return type.IsInterface
				? database.FirstOrDefault( e =>
					e.Info.HasInterface( type ) && !e.Info.IsAbstract && search.Invoke( e ) )
				: database.FirstOrDefault( e =>
					(type == e.Info || e.Info.IsSubclassOf( type )) && !e.Info.IsAbstract && search.Invoke( e ) );
		}

		public static Library Find<T>( this Libraries database ) where T : class
		{
			return database.Find( typeof( T ) );
		}

		public static Library Find<T>( this Libraries database, Func<Library, bool> search ) where T : class
		{
			return database.Find( typeof( T ), search );
		}

		public static IEnumerable<Library> GetAll<T>( this Libraries database ) where T : class
		{
			var type = typeof( T );
			return type.IsInterface ? database.Where( e => e.Info.HasInterface<T>() ) : database.Where( e => e.Info.IsSubclassOf( type ) );
		}
	}
}
