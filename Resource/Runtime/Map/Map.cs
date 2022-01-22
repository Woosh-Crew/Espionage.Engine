using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	[Title( "Map" ), Help( "Allows the loading of scene through a resource" )]
	public sealed partial class Map : IResource, IDisposable
	{
		public string Name { get; set; }
		public string Description { get; set; }

		private AssetBundle Bundle { get; set; }
		private Scene Scene { get; set; }

		public Map( string path )
		{
			Path = path;
			Database ??= new InternalDatabase();
			Database.Add( this );
		}

		public void Dispose()
		{
			Unload( () => Database.Remove( this ) );
		}

		//
		// IResource 
		//

		public string Path { get; }
		public bool IsLoading { get; private set; }

		public void Load( Action onLoad = null )
		{
			if ( IsLoading )
			{
				Debugging.Log.Warning( "Already loading map" );
				return;
			}

			IsLoading = true;

			if ( Bundle is null )
			{
				// Load Bundle
				var bundleLoadRequest = AssetBundle.LoadFromFileAsync( Path );
				bundleLoadRequest.completed += ( _ ) =>
				{
					// When we've finished loading the asset
					// bundle, go onto loading the scene itself
					Bundle = bundleLoadRequest.assetBundle;
					Debugging.Log.Info( "Finished Loading Bundle" );

					// Call load again now that we've already got the bundle
					Load( onLoad );
				};
			}

			var scenePath = Bundle.GetAllScenePaths()[0];
			var sceneLoadRequest = SceneManager.LoadSceneAsync( scenePath, LoadSceneMode.Single );

			sceneLoadRequest.completed += ( _ ) =>
			{
				// We've finished loading the scene.
				Debugging.Log.Info( "Finished Loading Scene" );
				onLoad?.Invoke();
				IsLoading = false;
				Scene = SceneManager.GetSceneByPath( scenePath );
			};
		}

		public void Unload( Action onUnload = null )
		{
			IsLoading = true;

			var request = Bundle.UnloadAsync( true );
			request.completed += ( e ) =>
			{
				onUnload?.Invoke();
				IsLoading = false;
			};
		}
	}
}
