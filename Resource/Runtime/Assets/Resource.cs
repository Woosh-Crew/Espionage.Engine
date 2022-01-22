using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Espionage.Engine.Resources
{
	[Title( "Resource" ), Help( "Allows loading precompiled assets at runtime" )]
	public class Resource<T> : IResource, IDisposable where T : Object
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
			Unload( () => Database.Remove( this ) );
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

		//
		// Database
		//

		public static IDatabase<IResource, string> Database { get; private set; }

		private class InternalDatabase : IDatabase<IResource, string>
		{
			public IEnumerable<IResource> All => _records.Values;
			private readonly Dictionary<string, IResource> _records = new();

			public IResource this[ string key ] => _records[key];

			public void Add( IResource item )
			{
				// Store it in Database
				if ( _records.ContainsKey( item.Path! ) )
				{
					_records[item.Path] = item;
				}
				else
				{
					_records.Add( item.Path!, item );
				}
			}

			public void Clear()
			{
				_records.Clear();
			}

			public bool Contains( IResource item )
			{
				return _records.ContainsKey( item.Path );
			}

			public void Remove( IResource item )
			{
				_records.Remove( item.Path );
			}
		}
	}
}
