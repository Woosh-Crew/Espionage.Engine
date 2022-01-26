using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class FileAttribute : Attribute, Library.IComponent
	{
		public string Name { get; set; }
		public string Extension { get; set; }

		public void OnAttached( ref Library library ) { }
	}
}
