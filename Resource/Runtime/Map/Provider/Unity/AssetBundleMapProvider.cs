using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	[Library, Title( "Asset Bundle Map" ), Group( "Maps" ), File( Extension = "umap", Serialization = "Unity" )]
	public class AssetBundleMapProvider : IMapProvider
	{
		public string Identifier => File.FullName;
		private FileInfo File { get; }

		// Outcome
		public Scene? Scene { get; private set; }

		// Loading Meta
		public float Progress => _bundleRequestOperation.progress / 2 + _sceneLoadOperation.progress / 2;
		public bool IsLoading { get; private set; }

		public AssetBundleMapProvider( FileInfo file )
		{
			File = file;
		}

		//
		// Resource
		//

		private AssetBundleCreateRequest _bundleRequestOperation;
		private AsyncOperation _sceneLoadOperation;

		private AssetBundle _bundle;

		public void Load( Action finished )
		{
			IsLoading = true;

			// Load Bundle
			_bundleRequestOperation = AssetBundle.LoadFromFileAsync( File.FullName );
			_bundleRequestOperation.completed += ( _ ) =>
			{
				// When we've finished loading the asset
				// bundle, go onto loading the scene itself
				_bundle = _bundleRequestOperation.assetBundle;
				Debugging.Log.Info( "Finished Loading Bundle" );

				// Load the scene by getting all scene
				// paths from a bundle, and getting the first index
				var scenePath = _bundle.GetAllScenePaths()[0];
				_sceneLoadOperation = SceneManager.LoadSceneAsync( scenePath, LoadSceneMode.Additive );
				_sceneLoadOperation.completed += ( _ ) =>
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

		public void Unload( Action finished )
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
