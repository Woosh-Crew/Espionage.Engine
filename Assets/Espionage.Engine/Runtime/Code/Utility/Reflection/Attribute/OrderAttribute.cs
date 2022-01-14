using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class OrderAttribute : Attribute, Library.IComponent
	{
		public Library Library { get; set; }

		public OrderAttribute( int order )
		{
			_order = order;
		}

		private int _order;
		public int Order => _order;
	}
}
