using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class LibraryAttribute : Attribute
	{
		public string Name { get; set; }
		public string Title { get; set; }
		public string Help { get; set; }
		public bool Spawnable { get; set; }

		public LibraryAttribute() { }

		public LibraryAttribute( string name )
		{
			this.Name = name;
		}

		public Library CreateRecord()
		{
			return new Library()
			{
				Name = Name,
				Help = Help,
				Title = Title,
				Spawnable = Spawnable,
			};
		}
	}
}
