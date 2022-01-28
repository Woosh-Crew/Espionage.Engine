using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
	public sealed class OrderAttribute : Attribute, IComponent<Library>, IComponent<Property>
	{
		public int Order { get; }

		public OrderAttribute( int order )
		{
			Order = order;
		}

		public void OnAttached( Library library ) { }
		public void OnAttached( Property property ) { }
	}
}
