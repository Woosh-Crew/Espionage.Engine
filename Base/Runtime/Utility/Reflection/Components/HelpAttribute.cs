using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class HelpAttribute : Attribute, IComponent<Library>, IComponent<Property>
	{
		private readonly string _help;

		public HelpAttribute( string help )
		{
			_help = help;
		}

		public void OnAttached( Library library )
		{
			library.Help = _help;
		}

		public void OnAttached( Property property )
		{
			property.Help = _help;
		}
	}
}
