using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Espionage.Engine.Resources.Binders
{
	public class AssetBundleUIBinder : UI.Binder
	{
		public override string Identifier { get; }

		public AssetBundleUIBinder( string path )
		{
			Identifier = path;
		}

		//
		// Resource
		//

		private AssetBundleCreateRequest _bundleRequestOperation;
		private AsyncOperation _sceneLoadOperation;

		private AssetBundle _bundle;

		public override void Load( Action onLoad = null )
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
				var asset = _bundle.LoadAllAssetsAsync<VisualTreeAsset>();
				_sceneLoadOperation.completed += ( _ ) =>
				{
					// We've finished loading the scene.
					Debugging.Log.Info( "Finished Loading UI" );
					onLoad?.Invoke();
				};
			};
		}

		public override void Unload( Action onUnload = null )
		{
			var request = _bundle.UnloadAsync( true );
			request.completed += ( e ) =>
			{
				Debugging.Log.Info( "Finished Unloading Bundle" );
				onUnload?.Invoke();
			};
		}
	}
}
