using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	[Library, Title( "Editor Map" ), Group( "Maps" )]
	public class EditorSceneMapProvider : Map.Service
	{
		public override string Identifier => "editor";

		public EditorSceneMapProvider()
		{
			_sceneName = SceneManager.GetActiveScene().name;
		}

		//
		// Resource
		//

		private readonly string _sceneName;

		public override void Load( Action<Scene> finished )
		{
			var operation = SceneManager.LoadSceneAsync( _sceneName, LoadSceneMode.Additive );
			operation.completed += ( _ ) =>
			{
				Scene = SceneManager.GetSceneByName( _sceneName );
				finished?.Invoke( Scene );
			};
		}

		public override void Unload( Action finished )
		{
			if ( Scene == default )
			{
				Scene = SceneManager.GetActiveScene();
			}

			var request = Scene.Unload();
			request.completed += _ => finished?.Invoke();
		}
	}
}
