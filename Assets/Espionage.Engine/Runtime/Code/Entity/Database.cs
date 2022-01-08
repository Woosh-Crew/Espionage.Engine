using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace Espionage.Engine.Entities
{
	[CreateAssetMenu( menuName = "Espionage.Engine/Entities/Database", fileName = "Entity Database" )]
	public class Database : ScriptableObject
	{
		public List<Blueprint> references;

		//
		// Editor
		//
#if UNITY_EDITOR

		[InitializeOnLoadMethod]
		private static void CreateInstance()
		{
			var path = Path.GetFullPath( $"{Application.dataPath}/Espionage.Engine/Runtime/Resources" );
		}

#endif
	}
}
