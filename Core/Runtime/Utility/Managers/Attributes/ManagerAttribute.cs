using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
	public sealed class ManagerAttribute : Attribute
	{
		public ManagerAttribute( string initializer )
		{
			Method = initializer;
		}

		public string Method { get; }

		public int Order { get; set; }
		public Layer Layer { get; set; } = Layer.Runtime;
	}
}
