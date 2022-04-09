using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources.Maps
{
	[Library, Title( "Asset Bundle Map" ), Group( "Maps" )]
	public class AssetBundleMapProvider : Map.Binder
	{
		public override float Progress => _operation.progress;

		public AssetBundle Bundle { get; }

		public AssetBundleMapProvider( AssetBundle bundle )
		{
			Bundle = bundle;
		}

		private AsyncOperation _operation;

		public override void Load( Action finished )
		{
			// Load the scene by getting all scene
			// paths from a bundle, and getting the first index
			var scenePath = Bundle.GetAllScenePaths()[0];
			_operation = SceneManager.LoadSceneAsync( scenePath, LoadSceneMode.Additive );
			_operation.completed += ( _ ) =>
			{
				Scene = SceneManager.GetSceneByPath( scenePath );
				SceneManager.SetActiveScene( Scene );

				OnLoad();

				// We've finished loading the scene.
				Debugging.Log.Info( "Finished Loading Scene" );
				finished?.Invoke();
			};
		}

		protected virtual void OnLoad() { }
	}
}
