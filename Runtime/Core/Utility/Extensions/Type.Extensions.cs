using System;
using System.Linq;

namespace Espionage.Engine
{
	public static class TypeExtensions
	{
		public static bool HasInterface<T>( this Type type )
		{
			return type.GetInterfaces().Contains( typeof( T ) );
		}

		public static bool HasInterface( this Type type, Type interfaceType )
		{
			return type.GetInterfaces().Contains( interfaceType );
		}
	}
}
