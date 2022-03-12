using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine
{
	public static class SceneExtensions
	{
		/// <summary> Unload this Scene. </summary>
		public static AsyncOperation Unload( this Scene self )
		{
			return self.isLoaded && self.IsValid() ? SceneManager.UnloadSceneAsync( self.name ) : null;
		}

		public static GameObject Grab( this Scene self, GameObject gameObject )
		{
			SceneManager.MoveGameObjectToScene( gameObject, self );
			return gameObject;
		}
	}
}
