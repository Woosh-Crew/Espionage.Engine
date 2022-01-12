using System;
using UnityEngine;

namespace Espionage.Engine.Entities
{
	[CreateAssetMenu( menuName = "Espionage.Engine/Blueprint", fileName = "Blueprint" )]
	[Library.Skip, Library.Constructor( nameof( Constructor ) )]
	public class Blueprint : ScriptableObject, ILibrary
	{
		[field: SerializeField]
		public Library ClassInfo { get; set; }

		public string entityReference;
		public GameObject prefab;

		public Blueprint()
		{
		}

		private static object Constructor()
		{
			throw new NotImplementedException();
		}
	}
}
