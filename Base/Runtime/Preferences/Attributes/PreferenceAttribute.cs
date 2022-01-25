using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Property )]
	public class PreferenceAttribute : Attribute
	{
		public string Title { get; set; }
		public string Tooltip { get; set; }
	}
}
