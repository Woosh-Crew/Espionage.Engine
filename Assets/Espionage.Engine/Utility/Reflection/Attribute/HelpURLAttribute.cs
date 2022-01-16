using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class HelpURLAttribute : Attribute, Library.IComponent
	{
		public Library Library { get; set; }

		public HelpURLAttribute( string url )
		{
			URL = url;
		}

		public string URL { get; }
	}
}
