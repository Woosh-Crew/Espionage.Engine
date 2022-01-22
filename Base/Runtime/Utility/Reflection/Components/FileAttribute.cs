using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
	public sealed class FileAttribute : Attribute, Library.IComponent, Property.IComponent
	{
		public string Name { get; set; }
		public string Extension { get; set; }

		public void OnAttached( ref Library library ) { }

		public void OnAttached( ref Property property ) { }
	}
}
