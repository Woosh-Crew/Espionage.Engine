using System;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources.Binders
{
	[Library, Title( "Editor Map" ), Group( "Maps" )]
	public class EditorSceneMapProvider : Map.Binder
	{
		public EditorSceneMapProvider()
		{
			_sceneName = SceneManager.GetActiveScene().name;
		}

		//
		// Resource
		//

		private readonly string _sceneName;

		public override void Load( Action finished )
		{
			var operation = SceneManager.LoadSceneAsync( _sceneName, LoadSceneMode.Additive );
			operation.completed += ( _ ) =>
			{
				Scene = SceneManager.GetSceneByName( _sceneName );
				finished?.Invoke();
			};
		}

		public override void Unload()
		{
			if ( Scene == default )
			{
				Scene = SceneManager.GetActiveScene();
			}

			Scene.Unload();
		}
	}
}
