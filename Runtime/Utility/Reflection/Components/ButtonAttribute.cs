using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// Reflection Component that changes the Tile value on a Library or Property.
	/// </summary>
	[AttributeUsage( AttributeTargets.Method, Inherited = false )]
	public sealed class ButtonAttribute : Attribute, IComponent<Function>
	{
		public string Title { get; }

		public ButtonAttribute( string title )
		{
			Title = title;
		}

		public void OnAttached( Function item ) { }
	}
}
