using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
	public sealed class OrderAttribute : Attribute, Library.IComponent, Property.IComponent
	{
		public int Order { get; }

		public OrderAttribute( int order )
		{
			Order = order;
		}

		public void OnAttached( ref Library library ) { }
		public void OnAttached( ref Property property ) { }
	}
}
