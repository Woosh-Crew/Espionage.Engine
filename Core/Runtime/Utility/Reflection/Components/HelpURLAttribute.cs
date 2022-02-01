using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class HelpURLAttribute : Attribute, IComponent<Library>, IComponent<Property>
	{
		public string URL { get; }

		public HelpURLAttribute( string url )
		{
			URL = url;
		}

		public void OnAttached( Library library ) { }
		public void OnAttached( Property property ) { }
	}
}
