using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine
{
	public static class GameObjectExtensions
	{
		public static GameObject MoveTo( this GameObject self, Scene scene )
		{
			SceneManager.MoveGameObjectToScene(self, scene);
			return self;
		}
	}
}
