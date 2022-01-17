using System;
using System.Linq;

public static class TypeExtensions
{
	public static bool HasInterface<T>( this Type type )
	{
		return type.GetInterfaces().Contains( typeof( T ) );
	}
}
