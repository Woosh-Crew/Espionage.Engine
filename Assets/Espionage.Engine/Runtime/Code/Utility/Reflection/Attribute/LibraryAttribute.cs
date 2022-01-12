using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class )]
	public class LibraryAttribute : Attribute
	{
		public string Name { get; set; }
		public string Title { get; set; }
		public string Help { get; set; }
		public string Icon { get; set; }

		public int Order { get; set; }

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
				Description = Help,
				Title = Title,
				Owner = type,
				Icon = Icon,

				Order = Order,
			};
		}
	}
}
