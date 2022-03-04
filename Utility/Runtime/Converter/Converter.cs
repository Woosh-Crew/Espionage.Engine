using System;

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
	}
}
