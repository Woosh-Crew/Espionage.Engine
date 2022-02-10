using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Blueprints are used for spawning prefabs at runtime using C#
	/// </summary>
	[Group( "Blueprints" )]
	public abstract class Blueprint : ILibrary
	{
		public Library ClassInfo { get; }

		public Blueprint()
		{
			ClassInfo = Library.Database[GetType()];
		}

		public GameObject Spawn()
		{
			return null;
		}
	}
}
