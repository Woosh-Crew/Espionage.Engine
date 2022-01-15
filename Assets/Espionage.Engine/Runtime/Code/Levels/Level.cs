using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	[CreateAssetMenu( menuName = "Espionage.Engine/Level" )]
	public class Level : ScriptableObject
	{
#if UNITY_EDITOR
		public UnityEditor.SceneAsset scene;
#endif

		public string title;
		public string description;
	}
}
