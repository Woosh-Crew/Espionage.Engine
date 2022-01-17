using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class HelpAttribute : Attribute, Library.IComponent, Property.IComponent
	{
		private readonly string _help;

		public HelpAttribute( string help )
		{
			_help = help;
		}

		public void OnAttached( ref Library library )
		{
			library.Help = _help;
		}

		public void OnAttached( ref Property property )
		{
			property.Help = _help;
		}
	}
}
