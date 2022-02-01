using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class LibraryAttribute : Attribute
	{
		private readonly string _name;

		public LibraryAttribute() { }

		public LibraryAttribute( string name )
		{
			_name = name;
		}

		public Library CreateRecord( Type type )
		{
			var library = new Library( type );

			// This looks really stupid
			if ( !string.IsNullOrEmpty( _name ) )
			{
				library.Name = _name;
			}

			return library;
		}
	}
}
