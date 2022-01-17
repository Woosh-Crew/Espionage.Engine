using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class OrderAttribute : Attribute, Library.IComponent
	{
		public int Order { get; }

		public OrderAttribute( int order )
		{
			Order = order;
		}

		public void OnAttached( ref Library library ) { }
	}
}
