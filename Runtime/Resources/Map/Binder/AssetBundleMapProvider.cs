﻿using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources.Binders
{
	[Library, Title( "Asset Bundle Map" ), Group( "Maps" )]
	public class AssetBundleMapProvider : Map.Binder
	{
		public override string Identifier { get; }

		// Loading Meta
		public override float Progress => _bundleRequestOperation.progress / 2 + _sceneLoadOperation.progress / 2;

		public AssetBundleMapProvider( string path )
		{
			Identifier = path;
		}

		//
		// Resource
		//

		private AssetBundleCreateRequest _bundleRequestOperation;
		private AsyncOperation _sceneLoadOperation;

		private AssetBundle _bundle;

		public override void Load( Action<Scene> finished = null )
		{
			// Load Bundle
			_bundleRequestOperation = AssetBundle.LoadFromFileAsync( Identifier );
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
					Scene = SceneManager.GetSceneByPath( scenePath );
					finished?.Invoke( Scene );
				};
			};
		}

		public override void Unload( Action finished = null )
		{
			// Unload scene and bundle
			Scene.Unload();
			Scene = default;

			var request = _bundle.UnloadAsync( true );
			request.completed += ( e ) =>
			{
				Debugging.Log.Info( "Finished Unloading Bundle" );
				finished?.Invoke();
			};
		}
	}
}