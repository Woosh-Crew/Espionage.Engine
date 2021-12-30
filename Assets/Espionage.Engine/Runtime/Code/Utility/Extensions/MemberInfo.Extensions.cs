using System;
using System.Collections.Generic;
using System.Reflection;

public static class MemberInfoExtensions
{
	public static Type[] GetParameterTypes( this MemberInfo info )
	{
		List<Type> paramteres = new List<Type>();

		if ( info is PropertyInfo propertyInfo )
			return new Type[] { propertyInfo.PropertyType };

		if ( info is MethodInfo methodInfo )
		{
			foreach ( var item in methodInfo.GetParameters() )
				paramteres.Add( item.ParameterType );
		}

		return paramteres.ToArray();
	}
}
