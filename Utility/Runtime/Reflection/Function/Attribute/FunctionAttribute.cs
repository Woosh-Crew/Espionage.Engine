using System;
using System.Reflection;

namespace Espionage.Engine
{
	/// <summary>
	/// Properties are variables that are changeable by the editor.
	/// </summary>
	[AttributeUsage( AttributeTargets.Method, Inherited = true, AllowMultiple = false )]
	public sealed class FunctionAttribute : Attribute
	{
		public string Name { get; set; }
		public string Title { get; set; }
		public string Group { get; set; }
		public string Help { get; set; }

		public FunctionAttribute() { }

		public FunctionAttribute( string name )
		{
			Name = name;
		}

		public Function CreateRecord( Library library, MethodInfo info )
		{
			return new Function( library, info )
			{
				Name = Name,
				Help = Help,
				Title = Title,
				Group = Group,
			};
		}
	}
}
