using System;
using System.Linq;
using System.Reflection;

namespace Espionage.Engine
{
	public static class TypeExtensions
	{
		public static bool HasInterface<T>( this Type type )
		{
			return type.GetInterfaces().Contains( typeof( T ) );
		}

		public static bool HasAttribute<T>( this MemberInfo type, bool inherited = false ) where T : Attribute
		{
			return type.IsDefined( typeof( T ), inherited );
		}

		public static bool HasInterface( this Type type, Type interfaceType )
		{
			return type.GetInterfaces().Contains( interfaceType );
		}
	}
}
