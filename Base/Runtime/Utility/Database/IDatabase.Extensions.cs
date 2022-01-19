using System;
using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine
{
	public static class DatabaseExtensions
	{
		public static T Add<T>( IDatabase<T> database )
		{
			var obj = Activator.CreateInstance<T>();
			database.Add( obj );
			return obj;
		}

		public static bool Contains<T>( IDatabase<T> database )
		{
			return database.All.FirstOrDefault( e => e is T ) != null;
		}
	}
}
