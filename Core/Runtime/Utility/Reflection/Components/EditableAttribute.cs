using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// Reflection component for letting this class or property be editable or not.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
	public sealed class EditableAttribute : Attribute, IComponent<Library>, IComponent<Property>
	{
		public bool Editable { get; }

		public EditableAttribute( bool editable = true )
		{
			Editable = editable;
		}

		public void OnAttached( Library library ) { }

		public void OnAttached( Property property )
		{
			property.Editable = Editable;
		}
	}
}
