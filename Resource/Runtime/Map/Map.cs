using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	/// <summary>
	/// A reference to a map file (.map).
	/// </summary>
	[Title( "Map" ), Group( "Maps" ), File( Extension = "map" ), Manager( nameof( Cache ), Order = 600, Layer = Layer.Editor | Layer.Runtime )]
	public sealed partial class Map : IResource, IDisposable, IAsset, ILibrary
	{
		public Library ClassInfo { get; }
		public static Map Current { get; private set; }

		//
		// Meta Data
		//

		public string Title { get; set; }
		public string Description { get; set; }

		/// <summary>
		/// Components contain map meta data.
		/// This could include a reference to a steam workshop item
		/// </summary>
		public IDatabase<IComponent> Components { get; }

		/// <summary>Make a map reference from a path.</summary>
		/// <param name="path">Where is the map located? Is relative to the game's directory</param>
		public Map( string path )
		{
			if ( !File.Exists( path ) )
			{
				Debugging.Log.Error( "Invalid Map Path" );
				throw new DirectoryNotFoundException();
			}

			ClassInfo = Library.Database.Get<Map>();
			Path = path;
			Database.Add( this );

			Components = new InternalComponentDatabase( this );
		}

		public static Map Find( string path )
		{
			return Database[path] ?? new Map( path );
		}

		//
		// Cache
		// 

		static Map()
		{
			Database = new InternalDatabase();
		}

		internal static void Cache()
		{
			using var stopwatch = Debugging.Stopwatch( "Caching Maps" );

			var path = Application.isEditor ? "Exports/Maps" : Application.dataPath;

			foreach ( var map in Directory.GetFiles( path, "*.map", SearchOption.AllDirectories ) )
			{
				Database.Add( new Map( map ) );
			}
		}

		//
		// Resource 
		//

		public string Path { get; }
		public bool IsLoading { get; private set; }

		private AssetBundle _bundle;
		private Scene? _scene;

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
				Debugging.Log.Warning( "Already performing an operation action this map" );
				return false;
			}

			IsLoading = true;

			// Unload the current map
			try
			{
				if ( Current != null )
				{
					return Current.Unload( () => Internal_LoadRequest( onLoad ) );
				}

				Internal_LoadRequest( onLoad );
			}
			finally
			{
				Current = this;
			}

			return true;
		}

		private void Internal_LoadRequest( Action onLoad = null )
		{
			// Load Bundle
			var bundleLoadRequest = AssetBundle.LoadFromFileAsync( Path );
			bundleLoadRequest.completed += ( _ ) =>
			{
				// When we've finished loading the asset
				// bundle, go onto loading the scene itself
				_bundle = bundleLoadRequest.assetBundle;
				Debugging.Log.Info( "Finished Loading Bundle" );

				// Load the scene by getting all scene
				// paths from a bundle, and getting the first index
				var scenePath = _bundle.GetAllScenePaths()[0];
				var sceneLoadRequest = SceneManager.LoadSceneAsync( scenePath, LoadSceneMode.Additive );
				sceneLoadRequest.completed += ( _ ) =>
				{
					// We've finished loading the scene.
					Debugging.Log.Info( "Finished Loading Scene" );
					onLoad?.Invoke();
					IsLoading = false;
					_scene = SceneManager.GetSceneByPath( scenePath );

					// Tell components we've loaded
					foreach ( var component in Components.All )
					{
						component.OnLoad();
					}
				};
			};
		}

		/// <summary>
		/// Unload this Resource from memory.
		/// </summary>
		/// <param name="onUnload">
		/// What to do when we finish unloading
		/// </param>
		public bool Unload( Action onUnload = null )
		{
			if ( IsLoading )
			{
				Debugging.Log.Warning( "Already performing an operation action this map" );
				return false;
			}

			if ( _bundle is null )
			{
				Debugging.Log.Warning( "Invalid Bundle. Cannot Unload" );
				return false;
			}

			IsLoading = true;

			if ( Current == this )
			{
				Current = null;
			}

			Internal_UnloadRequest( onUnload );

			return true;
		}

		private void Internal_UnloadRequest( Action onUnload = null )
		{
			// Unload scene and bundle
			_scene?.Unload();
			_scene = null;

			var request = _bundle.UnloadAsync( true );
			request.completed += ( e ) =>
			{
				Debugging.Log.Info( "Finished Unloading Bundle" );
				IsLoading = false;
				onUnload?.Invoke();

				// Tell components we've Unloaded
				foreach ( var component in Components.All )
				{
					component.OnUnload();
				}
			};
		}

		public void Dispose()
		{
			Unload( () => Database.Remove( this ) );
		}

		//
		// Components
		//

		public interface IComponent
		{
			void OnAttached( ref Map map );
			void OnDetached() { }

			void OnLoad() { }
			void OnUnload() { }
		}

		private class InternalComponentDatabase : IDatabase<IComponent>
		{
			public IEnumerable<IComponent> All => _components;

			public InternalComponentDatabase( Map map )
			{
				_target = map;
			}

			private Map _target;
			private readonly List<IComponent> _components = new();

			public void Add( IComponent item )
			{
				_components.Add( item );
				item.OnAttached( ref _target );
			}

			public void Clear()
			{
				foreach ( var item in _components )
				{
					Remove( item );
				}

				_components.Clear();
			}

			public bool Contains( IComponent item )
			{
				return _components.Contains( item );
			}

			public void Remove( IComponent item )
			{
				_components.Remove( item );
				item.OnDetached();
			}
		}

		//
		// Commands
		//

		[Debugging.Cmd( "map" )]
		private static void CmdGetMap()
		{
			if ( Current is null )
			{
				Debugging.Log.Info( "No Map" );
				return;
			}

			Debugging.Log.Info( $"[{Current.Title}] - {Current.Path}" );
		}

		[Debugging.Cmd( "map.load" )]
		private static void CmdLoadFromPath( string path )
		{
			Find( path )?.Load();
		}
	}
}
