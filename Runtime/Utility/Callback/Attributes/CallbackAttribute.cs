using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Method, Inherited = true, AllowMultiple = true )]
	public class CallbackAttribute : Attribute, IComponent<Function>
	{
		public string Name { get; }

		public CallbackAttribute( string name )
		{
			Name = name;
		}

		public void OnAttached( Function item )
		{
			Callback.Provider.Add( Name, item );
		}
	}
}
