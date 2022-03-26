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
		private string Name { get; }

		public FunctionAttribute() { }

		public FunctionAttribute( string name )
		{
			Name = name;
		}

		public Function CreateRecord( MethodInfo info )
		{
			return new( info, string.IsNullOrEmpty( Name ) ? info.Name : Name );
		}
	}
}
