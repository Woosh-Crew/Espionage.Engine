using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class HelpAttribute : Attribute, Library.IComponent
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
	}
}
