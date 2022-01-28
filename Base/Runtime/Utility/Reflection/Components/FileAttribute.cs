using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class FileAttribute : Attribute, IComponent<Library>
	{
		public string Name { get; set; }
		public string Extension { get; set; }
		public string Serialization { get; set; }

		public void OnAttached( Library library ) { }
	}
}
