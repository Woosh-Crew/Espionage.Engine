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

		public EditorSceneMapProvider()
		{
			_sceneName = SceneManager.GetActiveScene().name;
		}

		//
		// Resource
		//

		private readonly string _sceneName;

		public void Load( Action finished )
		{
			IsLoading = true;

			var operation = SceneManager.LoadSceneAsync( _sceneName, LoadSceneMode.Additive );
			operation.completed += ( _ ) =>
			{
				Output = SceneManager.GetSceneByName( _sceneName );
				SceneManager.SetActiveScene( Output );

				finished?.Invoke();
				IsLoading = false;
			};
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
