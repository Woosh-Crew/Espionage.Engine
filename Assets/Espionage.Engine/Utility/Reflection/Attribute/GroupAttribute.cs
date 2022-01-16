using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class GroupAttribute : Attribute, Library.IComponent
	{
		public Library Library { get; set; }

		public GroupAttribute( string group )
		{
			_group = group;
		}

		private readonly string _group;

		public void OnAttached()
		{
			Library.Group = _group;
		}
	}
}
