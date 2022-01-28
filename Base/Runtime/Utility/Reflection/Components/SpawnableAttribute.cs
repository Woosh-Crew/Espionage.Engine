using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class SpawnableAttribute : Attribute, IComponent<Library>
	{
		private readonly bool _spawnable;

		public SpawnableAttribute( bool spawnable = true )
		{
			_spawnable = spawnable;
		}

		public void OnAttached( Library library )
		{
			library.Spawnable = true;
		}
	}
}
