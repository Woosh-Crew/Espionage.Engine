using UnityEngine;

namespace Espionage.Engine
{
	[CreateAssetMenu( fileName = "Blueprint", menuName = "Espionage.Engine/Blueprint", order = 0 )]
	public class Blueprint : ScriptableObject
	{
		public Library library;
		public GameObject gameObject;
	}
}
