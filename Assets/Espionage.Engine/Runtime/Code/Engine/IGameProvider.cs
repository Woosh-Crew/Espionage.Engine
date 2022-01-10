using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine
{
	public interface IGameProvider
	{
		uint AppId { get; }

		void OnReady();
		void OnShutdown();
		void OnLevelLoaded( Scene lastScene, Scene newScene /* We should pass our own level class */ );
	}
}
