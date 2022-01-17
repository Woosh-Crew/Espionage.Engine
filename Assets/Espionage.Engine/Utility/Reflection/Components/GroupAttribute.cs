using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class GroupAttribute : Attribute, Library.IComponent
	{
		private readonly string _group;
		
		public GroupAttribute( string group )
		{
			_group = group;
		}

		public void OnAttached( ref Library library )
		{
			library.Group = _group;
		}
	}
}
