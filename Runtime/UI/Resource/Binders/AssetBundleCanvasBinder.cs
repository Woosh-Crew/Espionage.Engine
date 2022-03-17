using System;
using Espionage.Engine.Resources.Formats;
using UnityEngine;

namespace Espionage.Engine.Resources.Binders
{
	public class AssetBundleCanvasBinder : UI.Binder
	{
		public override string Identifier { get; }
		public override GameObject Canvas { get; set; }

		public AssetBundleCanvasBinder( string path )
		{
			Identifier = path;
		}

		//
		// Resource
		//

		private AssetBundleCreateRequest _bundleRequestOperation;
		private AssetBundle _bundle;

		public override void Load( Action<GameObject> onLoad = null )
		{
			// Load Bundle
			_bundleRequestOperation = AssetBundle.LoadFromFileAsync( Identifier );
			_bundleRequestOperation.completed += ( _ ) =>
			{
				// When we've finished loading the asset
				// bundle, go onto loading the scene itself
				_bundle = _bundleRequestOperation.assetBundle;
				Dev.Log.Info( "Finished Loading Bundle" );

				// Load the scene by getting all scene
				// paths from a bundle, and getting the first index
				var asset = _bundle.LoadAllAssetsAsync<CanvasAsset>();
				asset.completed += ( _ ) =>
				{
					// We've finished loading the scene.
					Dev.Log.Info( "Finished Loading UI" );
					Canvas = (asset.asset as CanvasAsset)?.UI.gameObject;
					onLoad?.Invoke( Canvas );
				};
			};
		}

		public override void Unload( Action onUnload = null )
		{
			var request = _bundle.UnloadAsync( true );
			request.completed += ( e ) =>
			{
				Dev.Log.Info( "Finished Unloading Bundle" );
				onUnload?.Invoke();
			};
		}
	}
}
