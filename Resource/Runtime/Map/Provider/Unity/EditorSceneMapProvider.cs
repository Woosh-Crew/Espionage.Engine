#if UNITY_EDITOR

using System;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	[Library, Title( "Editor Map" ), Group( "Maps" )]
	public class EditorSceneMapProvider : IMapProvider
	{
		// Id
		public string Identifier => "editor";

		// Outcome
		public Scene? Scene { get; private set; }

		// Loading Meta
		public float Progress { get; }
		public bool IsLoading { get; private set; }

		//
		// Editor Initialize
		//

		[RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.AfterSceneLoad )]
		private static void Init()
		{
			// Dont give a shit if this is hacky, its editor only
			var provider = new EditorSceneMapProvider { Scene = SceneManager.GetActiveScene() };
			Map.Current = new Map( provider );
			Map.Current.Load();
		}

		//
		// Resource
		//

		public void Load( Action finished )
		{
			IsLoading = true;

			Scene = SceneManager.GetActiveScene();
			finished?.Invoke();

			IsLoading = false;
		}

		public void Unload( Action finished )
		{
			IsLoading = true;

			Scene?.Unload();
			finished?.Invoke();

			IsLoading = false;
		}
	}
}

#endif
