using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Blueprints are used for spawning prefabs at runtime using C#
	/// </summary>
	[CreateAssetMenu, Group( "Blueprints" ), Spawnable( false )]
	public sealed class Blueprint : ScriptableObject, ILibrary
	{
		[SerializeField]
		private string identifier;

		[SerializeField]
		private GameObject prefab;

		// Class

		public Library ClassInfo { get; }

		private Blueprint()
		{
			ClassInfo = Library.Database[GetType()];
		}

		public GameObject Spawn()
		{
			return Instantiate( prefab );
		}
	}
}
