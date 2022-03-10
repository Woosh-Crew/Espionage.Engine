using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = true )]
	public class SingletonAttribute : Attribute, IComponent<Library>
	{
		public void OnAttached( Library item ) { }
	}
}
