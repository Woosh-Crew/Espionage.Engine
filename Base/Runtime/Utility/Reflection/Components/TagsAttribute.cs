using System;
using System.Linq;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
	public sealed class TagsAttribute : Attribute, Library.IComponent, Property.IComponent
	{
		public string[] Tags { get; }

		public TagsAttribute( params string[] tags )
		{
			Tags = tags;
		}

		public void OnAttached( ref Library library ) { }

		public void OnAttached( ref Property property ) { }
	}
}
