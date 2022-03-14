using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	[Library, Title( "Editor Map" ), Group( "Maps" )]
	public class EditorSceneMapProvider : Resource.IProvider<Map>
	{
		// Id
		public string Identifier => "editor";
		public float Progress { get; } = 1;

		public EditorSceneMapProvider()
		{
			_sceneName = SceneManager.GetActiveScene().name;
		}

		//
		// Resource
		//

		private Scene _scene;
		private readonly string _sceneName;

		public void Load( Action finished )
		{
			var operation = SceneManager.LoadSceneAsync( _sceneName, LoadSceneMode.Additive );
			operation.completed += ( _ ) =>
			{
				_scene = SceneManager.GetSceneByName( _sceneName );
				SceneManager.SetActiveScene( _scene );

				finished?.Invoke();
			};
		}

		public void Unload( Action finished )
		{
			if ( _scene == default )
			{
				_scene = SceneManager.GetActiveScene();
			}

			var request = _scene.Unload();
			request.completed += _ => finished?.Invoke();
		}
	}
}
