using System;
using UnityEngine;

namespace Espionage.Engine.Resources.Maps
{
	[Library( "map.asset_bundle" )]
	public class AssetBundleFile : Map.File
	{
		public AssetBundle Bundle { get; private set; }
		public override float Progress => _request?.progress ?? 0;

		private AssetBundleCreateRequest _request;

		public override void Load( Action loaded )
		{
			// Load Bundle
			_request = AssetBundle.LoadFromFileAsync( Info.FullName );
			_request.completed += _ =>
			{
				Bundle = _request.assetBundle;
				Binder = new AssetBundleMapProvider( Bundle );

				Debugging.Log.Info( $"Finished Loading Asset Bundle [{Info.Name}]" );
				loaded.Invoke();
			};
		}

		public override void Unload( Action finished )
		{
			if ( Bundle == null )
			{
				Debugging.Log.Warning( "Bundle was NULL?" );
				finished.Invoke();
				return;
			}

			Bundle.UnloadAsync( true ).completed += _ =>
			{
				Debugging.Log.Info( $"Finished Unloading Asset Bundle [{Info.Name}]" );
				finished.Invoke();
			};
		}
	}
}
