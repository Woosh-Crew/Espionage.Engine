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
		public string Path { get; }

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

		//
		// Spawners
		//

		/// <summary>
		/// <inheritdoc cref="Spawn"/>, and returns
		/// a component from it.
		/// </summary>
		/// <typeparam name="T">Component</typeparam>
		/// <returns>Component of type T</returns>
		public T Spawn<T>() where T : MonoBehaviour
		{
			return Spawn().GetComponent<T>();
		}

		/// <summary>
		/// Spawns the GameObject
		/// </summary>
		public GameObject Spawn()
		{
		#if UNITY_EDITOR
			var asset = AssetDatabase.LoadAssetAtPath<GameObject>( Path );
			var newObject = Object.Instantiate( asset );
			OnSpawn( newObject );
			return newObject;

		#elif UNITY_STANDALONE
			// Get Object from AssetBundle and Spawn
			return null;

		#else
			return null;

		#endif
		}

		/// <summary>
		/// Called when the object has just spawned.
		/// Use this for setting up predefined values.
		/// </summary>
		/// <param name="gameObject"> The Object that's being spawned </param>
		protected virtual void OnSpawn( GameObject gameObject )
		{
			gameObject.AddComponent<Identity>().Library = ClassInfo;
		}
	}
}
