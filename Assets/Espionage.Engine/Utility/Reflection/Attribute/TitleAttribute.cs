using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class TitleAttribute : Attribute, Library.IComponent
	{
		public Library Library { get; set; }

		public TitleAttribute( string title )
		{
			_title = title;
		}

		private readonly string _title;

		public void OnAttached()
		{
			Library.Title = _title;
		}
	}
}
