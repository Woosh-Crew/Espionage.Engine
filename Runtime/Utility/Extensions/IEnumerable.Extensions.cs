using System;
using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine
{
	public static class IEnumerableExtensions
	{
		public static T Random<T>( this IEnumerable<T> collection )
		{
			return collection.OrderBy( _ => Guid.NewGuid() ).FirstOrDefault();
		}
	}
}
