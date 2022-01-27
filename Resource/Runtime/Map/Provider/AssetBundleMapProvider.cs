using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	public class AssetBundleMapProvider : IMapProvider
	{
		public Scene? Scene { get; private set; }
		public bool IsLoading { get; private set; }

		//
		// Resource Loading & Unloading
		//

		private AssetBundle _bundle;

		public void Load( string path, Action finished )
		{
			IsLoading = true;

			// Load Bundle
			var bundleLoadRequest = AssetBundle.LoadFromFileAsync( path );
			bundleLoadRequest.completed += ( _ ) =>
			{
				// When we've finished loading the asset
				// bundle, go onto loading the scene itself
				_bundle = bundleLoadRequest.assetBundle;
				Debugging.Log.Info( "Finished Loading Bundle" );

				// Load the scene by getting all scene
				// paths from a bundle, and getting the first index
				var scenePath = _bundle.GetAllScenePaths()[0];
				var sceneLoadRequest = SceneManager.LoadSceneAsync( scenePath, LoadSceneMode.Additive );
				sceneLoadRequest.completed += ( _ ) =>
				{
					// We've finished loading the scene.
					Debugging.Log.Info( "Finished Loading Scene" );
					IsLoading = false;
					Scene = SceneManager.GetSceneByPath( scenePath );
					SceneManager.SetActiveScene( Scene.Value );
					finished?.Invoke();
				};
			};
		}

		public void Unload( string path, Action finished )
		{
			// Unload scene and bundle
			Scene?.Unload();
			Scene = null;

			var request = _bundle.UnloadAsync( true );
			request.completed += ( e ) =>
			{
				Debugging.Log.Info( "Finished Unloading Bundle" );
				IsLoading = false;
				finished?.Invoke();
			};
		}
	}
}
