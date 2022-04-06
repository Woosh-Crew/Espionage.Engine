using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// Reflection Component that says this class when created shouldn't
	/// be destroyed when switching Maps.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class , Inherited = true )]
	public class PersistentAttribute: Attribute, IComponent<Library>
	{
		public void OnAttached( Library library ) { }
	}
}
