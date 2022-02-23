using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// Reflection Component for storing what group does this class belong too.
	/// Will override the Library.Group value.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
	public sealed class GroupAttribute : Attribute, IComponent<Library>, IComponent<Property>, IComponent<Function>
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

		public void OnAttached( Function item )
		{
			item.Group = _group;
		}
	}
}
