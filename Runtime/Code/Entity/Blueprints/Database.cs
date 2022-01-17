using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace Espionage.Engine.Entities
{
	public class Database : ScriptableObject, IDatabase<Blueprint>
	{
		[SerializeField, HideInInspector]
		private List<Blueprint> references = new List<Blueprint>();

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

		public static Database Grab()
		{
			var assets = AssetDatabase.FindAssets( "t: Espionage.Engine.Entities.Database" );

			if ( assets.Length == 0 )
				return null;

			return AssetDatabase.LoadAssetAtPath<Database>( assets[0] );
		}

		private static void Build()
		{
			if ( AssetDatabase.FindAssets( "t: Espionage.Engine.Entities.Database" ).Length > 0 )
				return;

			var path = "Assets/Espionage.Engine.Cache/Resources";
			if ( !Directory.Exists( path ) )
				Directory.CreateDirectory( path );

			var database = ScriptableObject.CreateInstance<Database>();
			AssetDatabase.CreateAsset( database, $"{path}/Blueprint Database.asset" );

			foreach ( var item in AssetDatabase.FindAssets( "t: Espionage.Engine.Entities.Blueprint" ) )
			{
				database.Add( AssetDatabase.LoadAssetAtPath<Blueprint>( item ) );
			}

			AssetDatabase.SaveAssets();
		}

#endif
	}
}
