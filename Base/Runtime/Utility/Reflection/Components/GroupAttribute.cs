using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
	public sealed class GroupAttribute : Attribute, IComponent<Library>, IComponent<Property>
	{
		private readonly string _group;

		public GroupAttribute( string group )
		{
			_group = group;
		}

		public void OnAttached( Library library )
		{
			library.Group = _group;
		}

		public void OnAttached( Property property )
		{
			property.Group = _group;
		}
	}
}
