using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// Reflection Component that stores an order of operation.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Assembly )]
	public sealed class OrderAttribute : Attribute, IComponent<Library>, IComponent<Property>, IComponent<Function>
	{
		public int Order { get; }

		public OrderAttribute( int order )
		{
			Order = order;
		}

		public void OnAttached( Library library ) { }
		public void OnAttached( Property property ) { }
		public void OnAttached( Function item ) { }
	}
}
