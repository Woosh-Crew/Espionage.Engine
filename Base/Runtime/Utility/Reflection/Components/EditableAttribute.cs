using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class EditableAttribute : Attribute, Library.IComponent, Property.IComponent
	{
		public bool Editable { get; }

		public EditableAttribute( bool editable = true )
		{
			Editable = editable;
		}

		public void OnAttached( ref Library library ) { }
		public void OnAttached( ref Property property ) { }
	}
}
