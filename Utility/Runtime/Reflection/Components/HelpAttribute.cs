using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// Reflection Component that stores a help message or tooltip.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class HelpAttribute : Attribute, IComponent<Library>, IComponent<Property>, IComponent<Function>
	{
		public string URL { get; set; }

		public HelpAttribute( string help )
		{
			_help = help;
		}

		private readonly string _help;

		public void OnAttached( Library library )
		{
			library.Help = _help;
		}

		public void OnAttached( Property property )
		{
			property.Help = _help;
		}

		public void OnAttached( Function item )
		{
			item.Help = _help;
		}
	}
}
