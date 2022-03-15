using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	[Library, Title( "Asset Bundle Map" ), Group( "Maps" )]
	public class AssetBundleMapProvider : Resource.IProvider<Map, Scene>
	{
		public string Identifier => File.FullName;
		private FileInfo File { get; }

		// Loading Meta
		public float Progress => _bundleRequestOperation.progress / 2 + _sceneLoadOperation.progress / 2;

		public AssetBundleMapProvider( FileInfo file )
		{
			File = file;
		}

		//
		// Resource
		//

		private AssetBundleCreateRequest _bundleRequestOperation;
		private AsyncOperation _sceneLoadOperation;

		private Scene _scene;
		private AssetBundle _bundle;

		public void Load( Action<Scene> finished )
		{
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
					finished?.Invoke( SceneManager.GetSceneByPath( scenePath ) );
				};
			};
		}

		public void Unload( Action finished )
		{
			// Unload scene and bundle
			_scene.Unload();
			_scene = default;

			var request = _bundle.UnloadAsync( true );
			request.completed += ( e ) =>
			{
				Debugging.Log.Info( "Finished Unloading Bundle" );
				finished?.Invoke();
			};
		}
	}
}
