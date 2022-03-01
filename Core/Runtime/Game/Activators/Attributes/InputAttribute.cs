using System;
using Espionage.Engine.Components;

namespace Espionage.Engine.Activators
{
	public class InputAttribute : Attribute, IComponent<Function>
	{
		public void OnAttached( Function item ) {  }
	}
}
