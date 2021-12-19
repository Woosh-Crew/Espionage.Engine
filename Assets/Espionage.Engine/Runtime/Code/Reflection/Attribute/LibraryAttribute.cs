using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = false, AllowMultiple = false )]
	public class LibraryAttribute : Attribute
	{
		public string Name { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }

		public LibraryAttribute() { }

		public LibraryAttribute( string name )
		{
			this.Name = name;
		}

		public Library CreateRecord( Type type )
		{
			return new Library()
			{
				Name = Name,
				Title = Title,
				Owner = type,
			};
		}
	}
}
