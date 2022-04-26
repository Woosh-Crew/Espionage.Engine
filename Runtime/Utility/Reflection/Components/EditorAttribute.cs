using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// The editor attribute is used to mark a class as something that
	/// should be used in the editor as well as runtime, such as modules
	/// that need be created while in editor and runtime
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	public class EditorAttribute : Attribute, IComponent<Library>
	{
		public void OnAttached( Library item ) { }
	}
}
