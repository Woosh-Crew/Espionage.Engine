using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// Reflection Component that stores a help message or tooltip.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property )]
	public sealed class HelpAttribute : Attribute, IComponent<IMeta>
	{
		public string URL { get; set; }

		public HelpAttribute( string help )
		{
			_help = help;
		}

		private readonly string _help;

		public void OnAttached( IMeta library )
		{
			library.Help = _help;
		}
	}
}
