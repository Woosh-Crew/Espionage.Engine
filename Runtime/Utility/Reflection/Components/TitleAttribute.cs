using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// Reflection Component that changes the Tile value on a Library or Property.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method, Inherited = false )]
	public sealed class TitleAttribute : Attribute, IComponent<IMeta>
	{
		private readonly string _title;

		public TitleAttribute( string title )
		{
			_title = title;
		}

		public void OnAttached( IMeta library )
		{
			library.Title = _title;
		}
	}
}
