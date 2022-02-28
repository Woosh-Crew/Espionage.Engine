using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Method )]
	public class MenuAttribute : Attribute, IComponent<Function>
	{
		public string Path { get; }

		public MenuAttribute( string path )
		{
			Path = path;
		}

		public void OnAttached( Function item ) { }
	}
}
