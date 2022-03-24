using System;
using Espionage.Engine.Resources.Binders;
using UnityEngine;

namespace Espionage.Engine.Resources.Formats
{
	public class AssetBundleFile : Map.File
	{
		public AssetBundle Bundle { get; private set; }

		public override void Load( Action loaded )
		{
			// Load Bundle
			var request = AssetBundle.LoadFromFileAsync( Info.FullName );
			request.completed += _ =>
			{
				Bundle = request.assetBundle;
				Binder = new AssetBundleMapProvider( Bundle );

				Dev.Log.Info( $"Finished Loading Asset Bundle [{Info.Name}]" );
				loaded.Invoke();
			};
		}

		public override void Unload( Action finished )
		{
			Bundle.UnloadAsync( true ).completed += _ =>
			{
				Dev.Log.Info( $"Finished Unloading Asset Bundle [{Info.Name}]" );
				finished.Invoke();
			};
		}
	}
}
