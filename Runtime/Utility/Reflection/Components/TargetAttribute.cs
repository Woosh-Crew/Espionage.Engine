using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// Reflection Component for storing what group does this class belong too.
	/// Will override the Library.Group value.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class , Inherited = true )]
	public sealed class TargetAttribute : Attribute, IComponent<Library>
	{
		public Type Type { get; }

		public TargetAttribute( Type type )
		{
			Type = type;
		}

		public void OnAttached( Library library ) { }
	}
}
