using System;
using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Reflection Component that stores a reference to an Icon.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method )]
	public sealed class IconAttribute : Attribute, IComponent<Library>, IComponent<Property>, IComponent<Function>
	{
		public string Path { get; set; }

		public Texture2D Icon => null;

		public void OnAttached( Library library ) { }
		public void OnAttached( Property property ) { }
		public void OnAttached( Function item ) { }
	}
}
