using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	/// <summary>
	/// A reference to a map file (.map).
	/// </summary>
	[Title( "Map" ), Group( "Maps" ), File( Extension = "map" )]
	public sealed partial class Map : IResource, IDisposable, IAsset, ILibrary
	{
		public Library ClassInfo { get; }
		public static Map Current { get; private set; }

		//
		// Meta Data
		//

		public string Title { get; set; }
		public string Description { get; set; }

		/// <summary>Make a map reference from a path.</summary>
		/// <param name="path">Where is the map located? Is relative to the game's directory</param>
		public Map( string path )
		{
			if ( !Directory.Exists( path ) )
			{
				Debugging.Log.Error( "Invalid Map Path" );
				throw new DirectoryNotFoundException();
			}

			ClassInfo = Library.Database.Get<Map>();
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
		// Resource 
		//

		public string Path { get; }
		public bool IsLoading { get; private set; }

		private AssetBundle Bundle { get; set; }
		private Scene? Scene { get; set; }

		/// <summary>
		/// Loads the asset bundle if null, then will load the scene.
		/// Should be using this for loading map data and their scene.
		/// </summary>
		/// <param name="onLoad">
		/// What to do when we finish loading both the scene and asset bundle
		/// </param>
		public bool Load( Action onLoad = null )
		{
			if ( IsLoading )
			{
				Debugging.Log.Warning( "Already performing an operation action on map" );
				return false;
			}

			IsLoading = true;

			// Unload the current map
			Current?.Unload();
			Current = this;

			// Load Bundle
			var bundleLoadRequest = AssetBundle.LoadFromFileAsync( Path );
			bundleLoadRequest.completed += ( _ ) =>
			{
				// When we've finished loading the asset
				// bundle, go onto loading the scene itself
				Bundle = bundleLoadRequest.assetBundle;
				Debugging.Log.Info( "Finished Loading Bundle" );

				// Load the scene by getting all scene
				// paths from a bundle, and getting the first index
				var scenePath = Bundle.GetAllScenePaths()[0];
				var sceneLoadRequest = SceneManager.LoadSceneAsync( scenePath, LoadSceneMode.Additive );
				sceneLoadRequest.completed += ( _ ) =>
				{
					// We've finished loading the scene.
					Debugging.Log.Info( "Finished Loading Scene" );
					onLoad?.Invoke();
					IsLoading = false;
					Scene = SceneManager.GetSceneByPath( scenePath );
				};
			};

			return true;
		}

		/// <summary>
		/// Unload this Resource from memory.
		/// </summary>
		/// <param name="onUnload">What to do when we finish unloading</param>
		public bool Unload( Action onUnload = null )
		{
			if ( IsLoading )
			{
				Debugging.Log.Warning( "Already performing an operation action on map" );
				return false;
			}

			if ( Bundle is null )
			{
				Debugging.Log.Warning( "Invalid Bundle. Cannot Unload" );
				return false;
			}

			IsLoading = true;

			// Unload scene and bundle
			Scene?.Unload();
			Scene = null;

			var request = Bundle.UnloadAsync( true );
			request.completed += ( e ) =>
			{
				onUnload?.Invoke();
				IsLoading = false;
			};

			return true;
		}
	}
}
