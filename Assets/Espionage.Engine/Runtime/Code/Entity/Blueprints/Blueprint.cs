using UnityEngine;

namespace Espionage.Engine.Entities
{
	[CreateAssetMenu( menuName = "Espionage.Engine/Blueprint", fileName = "Blueprint" )]
	public class Blueprint : ScriptableObject
	{
		public string identifier;
		public string title;
		public string description;

		public string entityReference;
		public GameObject prefab;
	}
}
