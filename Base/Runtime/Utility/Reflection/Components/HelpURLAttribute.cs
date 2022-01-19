using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class HelpURLAttribute : Attribute, Library.IComponent, Property.IComponent
	{
		public string URL { get; }

		public HelpURLAttribute( string url )
		{
			URL = url;
		}

		public void OnAttached( ref Library library ) { }
		public void OnAttached( ref Property property ) { }
	}
}
