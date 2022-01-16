using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class SpawnableAttribute : Attribute, Library.IComponent
	{
		public Library Library { get; set; }

		public SpawnableAttribute( bool spawnable )
		{
			_spawnable = spawnable;
		}

		private bool _spawnable;

		public void OnAttached()
		{
			Library.Spawnable = _spawnable;
		}
	}
}
