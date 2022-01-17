using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
	public sealed class GroupAttribute : Attribute, Library.IComponent, Property.IComponent
	{
		private readonly string _group;

		public GroupAttribute( string group )
		{
			_group = group;
		}

		public void OnAttached( ref Library library )
		{
			library.Group = _group;
		}

		public void OnAttached( ref Property property )
		{
			property.Group = _group;
		}
	}
}
