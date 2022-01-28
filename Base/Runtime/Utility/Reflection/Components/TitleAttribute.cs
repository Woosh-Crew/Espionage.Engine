using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
	public sealed class TitleAttribute : Attribute, IComponent<Library>, IComponent<Property>
	{
		private readonly string _title;

		public TitleAttribute( string title )
		{
			_title = title;
		}

		public void OnAttached( Library library )
		{
			library.Title = _title;
		}

		public void OnAttached( Property property )
		{
			property.Title = _title;
		}
	}
}
