using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class TitleAttribute : Attribute, Library.IComponent
	{
		private readonly string _title;

		public TitleAttribute( string title )
		{
			_title = title;
		}

		public void OnAttached( ref Library library )
		{
			library.Title = _title;
		}
	}
}
