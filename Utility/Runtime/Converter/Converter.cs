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

			return converter.Convert( value );
		}
	}
}
