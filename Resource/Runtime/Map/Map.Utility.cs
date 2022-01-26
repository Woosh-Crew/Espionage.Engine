using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	public static class MapExtensions
	{
		public static T Get<T>( this IDatabase<Map.IComponent> database ) where T : Map.IComponent
		{
			return (T)database.All.FirstOrDefault( e => e is T );
		}

		public static IEnumerable<T> GetAll<T>( this IDatabase<Map.IComponent> database ) where T : Map.IComponent
		{
			return database.All.Select( e => (T)e );
		}

		public static bool TryGet<T>( this IDatabase<Map.IComponent> database, out T component ) where T : Map.IComponent
		{
			component = database.Get<T>();
			return component is not null;
		}

		public static bool Exists<T>( this IDatabase<Map.IComponent> database ) where T : Map.IComponent
		{
			return database.Get<T>() is not null;
		}
	}
}
