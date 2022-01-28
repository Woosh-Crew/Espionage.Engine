using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class EditableAttribute : Attribute, IComponent<Library>, IComponent<Property>
	{
		public bool Editable { get; }

		public EditableAttribute( bool editable = true )
		{
			Editable = editable;
		}

		public void OnAttached( Library library ) { }
		public void OnAttached( Property property ) { }
	}
}
