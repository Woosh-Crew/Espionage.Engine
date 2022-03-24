using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources.Binders
{
	[Library, Title( "Asset Bundle Map" ), Group( "Maps" )]
	public class AssetBundleMapProvider : Map.Binder
	{
		public override string Identifier { get; }
		public override float Progress => _operation.progress;

		public AssetBundle Bundle { get; }

		public AssetBundleMapProvider( AssetBundle bundle )
		{
			Bundle = bundle;
		}

		//
		// Resource
		//

		private AsyncOperation _operation;

		public override void Load( Action finished )
		{
			// Load the scene by getting all scene
			// paths from a bundle, and getting the first index
			var scenePath = Bundle.GetAllScenePaths()[0];
			_operation = SceneManager.LoadSceneAsync( scenePath, LoadSceneMode.Additive );
			_operation.completed += ( _ ) =>
			{
				// We've finished loading the scene.
				Dev.Log.Info( "Finished Loading Scene" );
				Scene = SceneManager.GetSceneByPath( scenePath );
				finished?.Invoke();
			};
		}
	}
}
