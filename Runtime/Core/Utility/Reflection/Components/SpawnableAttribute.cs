using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// Reflection Component that overrides the spawnable field on a library.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class SpawnableAttribute : Attribute, IComponent<Library>
	{
		private readonly bool _spawnable;

		public SpawnableAttribute( bool spawnable = true )
		{
			_spawnable = spawnable;
		}

		public void OnAttached( Library library )
		{
			library.Spawnable = _spawnable;
		}
	}
}
