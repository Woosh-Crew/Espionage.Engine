#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	[Library, Title( "Editor Map" ), Group( "Maps" )]
	public class EditorSceneMapProvider : Resource.IProvider<Map, Scene>
	{
		// Id
		public string Identifier => "editor";

		// Outcome
		public Scene Output { get; private set; }

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
			var provider = new EditorSceneMapProvider
			{
				Output = SceneManager.GetActiveScene()
			};
			Map.Current = new( provider );
			Map.Current.Load();
		}

		//
		// Resource
		//

		public void Load( Action finished )
		{
			IsLoading = true;

			Output = SceneManager.GetActiveScene();
			finished?.Invoke();

			IsLoading = false;
		}

		public void Unload( Action finished )
		{
			IsLoading = true;

			var request = Output.Unload();
			request.completed += _ => finished?.Invoke();

			IsLoading = false;
		}
	}
}

#endif
