using System;
using System.Reflection;

namespace Espionage.Engine
{
	/// <summary>
	/// Properties are variables that are changeable by the editor.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property, Inherited = true, AllowMultiple = false )]
	public sealed class PropertyAttribute : Attribute
	{
		public string Name { get; set; }
		public string Title { get; set; }
		public string Group { get; set; }
		public string Help { get; set; }
		public bool Serialized { get; set; }

		public PropertyAttribute() { }

		public Property CreateRecord( Library library, PropertyInfo info )
		{
			return new Property( library, info )
			{
				Name = Name,
				Help = Help,
				Title = Title,
				Group = Group,
				Serialized = Serialized
			};
		}
	}
}
