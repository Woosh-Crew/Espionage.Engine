using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class LibraryAttribute : Attribute
	{
		public string Name { get; set; }
		public string Title { get; set; }
		public string Group { get; set; }
		public string Help { get; set; }
		public bool Spawnable { get; set; } = true;

		public LibraryAttribute() { }

		public LibraryAttribute( string name )
		{
			Name = name;
		}

		public Library CreateRecord( Type type )
		{
			return new Library( type )
			{
				Name = Name,
				Help = Help,
				Title = Title,
				Spawnable = Spawnable,
				Group = Group
			};
		}
	}
}
