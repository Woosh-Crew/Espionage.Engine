using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class HelpAttribute : Attribute, Library.IComponent
	{
		public Library Library { get; set; }

		public HelpAttribute( string help )
		{
			_help = help;
		}

		private readonly string _help;

		public void OnAttached()
		{
			Library.help = _help;
		}
	}
}
