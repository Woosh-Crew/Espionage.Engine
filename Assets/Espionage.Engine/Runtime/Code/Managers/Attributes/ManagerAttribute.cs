using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class ManagerAttribute : Attribute
	{
		public ManagerAttribute( string initializer )
		{
			init = initializer;
		}

		private string init;
		public string Method => init;
		public Layer Layer { get; set; }
	}
}
