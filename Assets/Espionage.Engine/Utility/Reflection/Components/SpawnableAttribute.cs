using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class SpawnableAttribute : Attribute, Library.IComponent
	{
		private readonly bool _spawnable;

		public SpawnableAttribute( bool spawnable )
		{
			_spawnable = spawnable;
		}

		public void OnAttached( ref Library library )
		{
			library.Spawnable = _spawnable;
		}
	}
}
