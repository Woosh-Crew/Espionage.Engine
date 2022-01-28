using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine.Components
{
	public static class ComponentExtensions
	{
		public static T Get<T>( this IDatabase<IComponent, T2> database )
		{
			return (T)database.All.FirstOrDefault( e => e is T );
		}

		public static IEnumerable<T> GetAll<T>( this IDatabase<IComponent> database ) where T : IComponent
		{
			return database.All.Select( e => (T)e );
		}

		public static bool TryGet<T>( this IDatabase<IComponent> database, out T component ) where T :IComponent
		{
			component = database.Get<T>();
			return component is not null;
		}

		public static bool Exists<T>( this IDatabase<IComponent> database ) where T :IComponent
		{
			return database.Get<T>() is not null;
		}
	}
}
