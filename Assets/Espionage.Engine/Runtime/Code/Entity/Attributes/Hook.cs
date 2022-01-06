using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true, AllowMultiple = false )]
	public class HookAttribute : Attribute
	{
		public Type[] Hooks => components;
		readonly Type[] components;

		public HookAttribute( params Type[] components )
		{
			this.components = components;
		}
	}
}
