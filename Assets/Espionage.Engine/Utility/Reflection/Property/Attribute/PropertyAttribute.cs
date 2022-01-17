using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Property, Inherited = true, AllowMultiple = false )]
	sealed class PropertyAttribute : Attribute
	{
		public PropertyAttribute()
		{
		}
	}
}
