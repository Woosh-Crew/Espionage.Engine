using System;
using System.Linq;
using UnityEngine;

namespace Espionage.Engine.Resources.Models
{
	[Library( "mdl.asset_bundle" )]
	public abstract class AssetBundleFile : Model.File
	{
		public AssetBundle Bundle { get; private set; }

		private AssetBundleCreateRequest _request;

		public override void Load( Action<GameObject> loaded )
		{
			// Load Bundle
			Bundle = AssetBundle.LoadFromFile( Info.FullName );
			var gameObject = Bundle.LoadAllAssets<GameObject>().FirstOrDefault();
			loaded?.Invoke( gameObject );
			Dev.Log.Info( "Bundle Loaded" );
		}

		public override void Unload()
		{
			if ( Bundle == null )
			{
				Dev.Log.Warning( "Bundle was NULL?" );
				return;
			}

			Bundle.UnloadAsync( true ).completed += _ =>
			{
				Dev.Log.Info( $"Finished Unloading Asset Bundle [{Info.Name}]" );
			};
		}
	}
}
