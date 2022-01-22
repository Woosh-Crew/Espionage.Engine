using System;
using System.Collections.Generic;
using System.IO;
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

		/// <summary>Make a map reference from a path.</summary>
		/// <param name="path">Where is the map located? Is relative to the game's directory</param>
		public Map( string path )
		{
			if ( !Directory.Exists( path ) )
			{
				Debugging.Log.Error( "Invalid Map Path" );
				throw new DirectoryNotFoundException();
			}

			Path = path;
			Database.Add( this );
		}

		static Map()
		{
			Database = new InternalDatabase();
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

		/// <summary>
		/// Loads the asset bundle if null, then will load the scene.
		/// Should be using this for loading map data and their scene.
		/// </summary>
		/// <param name="onLoad">
		/// What to do when we finish loading both the scene and asset bundle
		/// </param>
		public void Load( Action onLoad = null )
		{
			if ( IsLoading )
			{
				Debugging.Log.Warning( "Already performing a loading action on map" );
				return;
			}

			IsLoading = true;

			// If there is no bundle
			// We will load it and call the method again
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

			// Load the scene by getting all scene
			// paths from a bundle, and getting the first index
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

		/// <summary>
		/// Unload this Resource from memory.
		/// This will unload the bundle itself, not the scene.
		/// use Scene.Unload for that.
		/// </summary>
		/// <param name="onUnload">What to do when we finish unloading</param>
		public void Unload( Action onUnload = null )
		{
			if ( IsLoading )
			{
				Debugging.Log.Warning( "Already performing a loading action on map" );
				return;
			}

			if ( Bundle is null )
			{
				Debugging.Log.Warning( "Invalid Bundle. Cannot Unload" );
				return;
			}

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
