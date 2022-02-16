using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Blueprints are used for spawning prefabs at runtime using C#
	/// </summary>
	[Group( "Blueprints" ), Spawnable( false )]
	public abstract class Blueprint : ILibrary
	{
		public Library ClassInfo { get; private set; }

		public Blueprint()
		{
			ClassInfo = Library.Database[GetType()];

			if ( !ClassInfo.Components.TryGet<FileAttribute>( out var fileAttribute ) )
			{
				Debugging.Log.Error( $"{ClassInfo.Name} doesn't have, FileAttribute" );
				return;
			}

			Path = fileAttribute.Path;
		}

		public string Path { get; }

		public GameObject Spawn()
		{
		#if UNITY_EDITOR
			var asset = AssetDatabase.LoadAssetAtPath<GameObject>( Path );
			var newObject = Object.Instantiate( asset );
			newObject.AddComponent<Identity>().Library = ClassInfo;
			return newObject;

		#elif UNITY_STANDALONE
			// Get Object from AssetBundle and Spawn

		#else
			return null;

		#endif
		}
	}
}
