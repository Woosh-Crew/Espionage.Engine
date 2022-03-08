using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Espionage.Engine.Resources
{
	[Library, Title( "Asset Bundle Model" ), Group( "Models" )]
	public class AssetBundleModelProvider : Resource.IProvider<Model, GameObject>
	{
		public string Identifier => File.FullName;
		private FileInfo File { get; }

		// Outcome
		public GameObject Output { get; private set; }


		// Loading Meta
		public float Progress => _bundleRequestOperation.progress / 2 + _modelLoadOperation.progress / 2;
		public bool IsLoading { get; private set; }

		public AssetBundleModelProvider( FileInfo file )
		{
			File = file;
		}

		//
		// Resource
		//

		private AssetBundleCreateRequest _bundleRequestOperation;
		private AssetBundleRequest _modelLoadOperation;

		private AssetBundle _bundle;

		public void Load( Action onLoad = null )
		{
			IsLoading = true;

			// This should be async...
			_bundle = AssetBundle.GetAllLoadedAssetBundles().FirstOrDefault( e => e.name == File.Name );

			if ( _bundle == null )
			{
				_bundle = AssetBundle.LoadFromFile( File.FullName );
			}

			Output = _bundle.LoadAsset<ModelAsset>( _bundle.GetAllAssetNames()[0] ).model;

			IsLoading = false;
			onLoad?.Invoke();
		}

		public void Unload( Action onUnload = null )
		{
			var request = _bundle.UnloadAsync( true );
			request.completed += ( e ) =>
			{
				Debugging.Log.Info( "Unloaded Model Bundle" );
				IsLoading = false;
				onUnload?.Invoke();
			};
		}
	}
}
