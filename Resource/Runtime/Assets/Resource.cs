using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Espionage.Engine.Resources
{
	[Title( "Resource" ), Help( "Allows loading precompiled assets at runtime" )]
	public partial class Resource<T> : IResource, IDisposable where T : Object
	{
		public T Asset { get; private set; }

		public Resource( string path )
		{
			if ( !Directory.Exists( path ) )
			{
				Debugging.Log.Error( "Invalid Resource Path" );
				throw new DirectoryNotFoundException();
			}

			Path = path;
			Database.Add( this );
		}

		static Resource()
		{
			Database = new InternalDatabase();
		}

		public void Dispose()
		{
			if ( Bundle != null )
			{
				Unload( () => Database.Remove( this ) );
			}
			else
			{
				Database.Remove( this );
			}
		}

		//
		// Resource
		//

		public string Path { get; }
		public bool IsLoading { get; private set; }
		private AssetBundle Bundle { get; set; }

		public virtual bool Load( Action onLoad = null )
		{
			if ( IsLoading )
			{
				Debugging.Log.Warning( "Already performing an operation action on map" );
				return false;
			}

			IsLoading = true;

			// Load Bundle
			var bundleLoadRequest = AssetBundle.LoadFromFileAsync( Path );
			bundleLoadRequest.completed += ( _ ) =>
			{
				// When we've finished loading the asset
				// bundle, go onto loading the scene itself
				Bundle = bundleLoadRequest.assetBundle;
				Debugging.Log.Info( "Finished Loading Bundle" );

				Asset = Bundle.LoadAsset<T>( Bundle.GetAllAssetNames()[0] );

				IsLoading = false;
			};

			return true;
		}

		public virtual bool Unload( Action onUnload = null )
		{
			if ( IsLoading )
			{
				Debugging.Log.Warning( "Already performing an operation action on map" );
				return false;
			}

			IsLoading = true;

			// Unload Bundle
			var request = Bundle.UnloadAsync( true );
			request.completed += ( _ ) =>
			{
				Debugging.Log.Info( "Finished Unloading Bundle" );
				Asset = null;
				IsLoading = false;
			};

			return true;
		}
	}
}
