using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class HelpURLAttribute : Attribute, Library.IComponent
	{
		public string URL { get; }

		public HelpURLAttribute( string url )
		{
			URL = url;
		}

		public void OnAttached( ref Library library ) { }
	}
}
