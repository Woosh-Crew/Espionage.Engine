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
	public class Database : ScriptableObject, IDatabase<Blueprint>
	{
		[SerializeField]
		private List<Blueprint> references;

		public IEnumerable<Blueprint> All => references;

		public void Add( Blueprint item )
		{
			references.Add( item );
		}

		public void Clear()
		{
			references.Clear();
		}

		public bool Contains( Blueprint item )
		{
			return references.Contains( item );
		}

		public void Remove( Blueprint item )
		{
			references.Remove( item );
		}

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
