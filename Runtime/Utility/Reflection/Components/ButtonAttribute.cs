using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// Reflection Component that adds a button the inspector,
	/// that'll invoke the attached function when clicked.
	/// </summary>
	[AttributeUsage( AttributeTargets.Method, Inherited = true )]
	public sealed class ButtonAttribute : Attribute, IComponent<Function>
	{
		public void OnAttached( Function item ) { }
	}
}
