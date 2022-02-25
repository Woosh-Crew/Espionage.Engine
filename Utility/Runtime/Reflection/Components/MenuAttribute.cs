using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	public class MenuAttribute : Attribute, IComponent<Function>
	{
		public void OnAttached( Function item ) {  }
	}
}
