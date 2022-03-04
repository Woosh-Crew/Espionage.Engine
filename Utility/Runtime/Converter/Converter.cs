using System;
using System.Reflection;

namespace Espionage.Engine
{
	/// <summary>
	/// Espionage.Engine's string to object
	/// converter.
	/// </summary>
	public static class Converter
	{
		public static T Convert<T>( string value )
		{
			var library = Library.Database.Find<IConverter<T>>();

			if ( library == null )
			{
				throw new Exception( "No Valid Converters for this Type" );
			}

			var converter = Library.Database.Create<IConverter<T>>( library.Class );

			try
			{
				return converter.Convert( value );
			}
			catch ( Exception e )
			{
				Debugging.Log.Exception( e );
				return default;
			}
		}

		public static object Convert( string value, Type type )
		{
			// JAKE: This is so aids.... But can't do much about that.

			var interfaceType = typeof( IConverter<> ).MakeGenericType( type );
			var library = Library.Database.Find( interfaceType );

			if ( library == null )
			{
				throw new Exception( "No Valid Converters for this Type" );
			}

			var converter = Library.Database.Create( library.Class );
			var method = interfaceType.GetMethod( "Convert", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );

			try
			{
				return method?.Invoke( converter, new object[] { value } );
			}
			catch ( Exception e )
			{
				Debugging.Log.Exception( e );
				return default;
			}
		}
	}
}
