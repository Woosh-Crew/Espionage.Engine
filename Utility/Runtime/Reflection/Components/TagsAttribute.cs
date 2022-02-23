using System;
using System.Linq;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// Reflection Component for storing Tags / Aliases.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
	public sealed class TagsAttribute : Attribute, IComponent<Library>, IComponent<Property>, IComponent<Function>
	{
		public string[] Tags { get; }

		public TagsAttribute( params string[] tags )
		{
			Tags = tags;
		}

		public void OnAttached( Library library ) { }

		public void OnAttached( Property property ) { }
		public void OnAttached( Function item ) { }
	}
}
