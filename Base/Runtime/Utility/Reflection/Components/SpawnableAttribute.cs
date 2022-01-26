using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class SpawnableAttribute : Attribute, Library.IComponent
	{
		private readonly bool _spawnable;

		public SpawnableAttribute( bool spawnable = true )
		{
			_spawnable = spawnable;
		}

		public void OnAttached( ref Library library )
		{
			library.Spawnable = true;
		}
	}
}
