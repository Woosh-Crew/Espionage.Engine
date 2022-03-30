using System;
using System.Linq;
using System.Reflection;

namespace Espionage.Engine
{
	public static class TypeExtensions
	{
		// Interfaces

		public static bool HasInterface<T>( this Type type )
		{
			return type.GetInterfaces().Contains( typeof( T ) );
		}

		public static Type GetInterface<T>( this Type type )
		{
			return type.GetInterfaces().FirstOrDefault( e => e == typeof( T ) );
		}

		public static bool HasInterface( this Type type, Type interfaceType )
		{
			return type.GetInterfaces().Contains( interfaceType );
		}

		public static Type GetInterface( this Type type, Type interfaceType )
		{
			return type.GetInterfaces().FirstOrDefault( e => e == interfaceType );
		}

		// Attribute

		public static bool HasAttribute<T>( this MemberInfo type, bool inherited = false ) where T : Attribute
		{
			return type.IsDefined( typeof( T ), inherited );
		}
	}
}
