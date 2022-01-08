using UnityEngine;

namespace Espionage.Engine.Entities
{
	[CreateAssetMenu( menuName = "Espionage.Engine/Entity", fileName = "Entity" )]
	public class Reference : ScriptableObject
	{
		public string identifier;
		public string title;
		public string description;

		public string entityReference;
		public GameObject prefab;
	}
}
