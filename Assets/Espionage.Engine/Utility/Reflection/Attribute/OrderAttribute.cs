using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class OrderAttribute : Attribute, Library.IComponent
	{
		public Library Library { get; set; }

		public OrderAttribute( int order )
		{
			Order = order;
		}

		public int Order { get; }
	}
}
