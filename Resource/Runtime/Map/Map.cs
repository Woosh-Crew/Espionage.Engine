using System;
using System.IO;
using Espionage.Engine.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	/// <summary>
	/// <para>
	/// Allows the loading and unloading of maps at runtime.
	/// </para>
	/// </summary>
	/// <remarks>
	/// You should be using this instead of UnityEngine.SceneManager.
	/// </remarks>
	[Group( "Maps" )]
	public sealed partial class Map : IResource, IDisposable, ILibrary, ILoadable
	{
		public static Map Current { get; internal set; }

		// Provider
		private IMapProvider Provider { get; }
		public ComponentDatabase<Map> Components { get; }

		// Meta Data
		public string Identifier => Provider.Identifier;
		public string Title { get; set; }
		public string Description { get; set; }

		// Loadable 
		float ILoadable.Progress => Provider.Progress;
		string ILoadable.Text => $"Loading {Title}";

		//
		// Constructors
		//

		public Library ClassInfo { get; }

		/// <summary>Make a reference to a map, using a provider.</summary>
		/// <param name="provider">What provider should we use for loading and unloading maps?</param>
		public Map( IMapProvider provider )
		{
			ClassInfo = Library.Database[GetType()];
			Components = new ComponentDatabase<Map>( this );

			Provider = provider;
			Database.Add( this );
		}

		/// <summary>
		/// Gets the map at a path.
		/// </summary>
		/// <param name="path">The path to this map.</param>
		/// <param name="noneFound">If there is no map found, what do we do?</param>
		public static Map Find( string path, Func<Map> noneFound = null )
		{
			return Database[path] ?? noneFound?.Invoke();
		}

		//
		// Resource 
		//

		public Action OnLoad { get; set; }
		public Action OnUnload { get; set; }

		public bool IsLoading => Provider.IsLoading;

		/// <summary>
		/// Loads this map. Behind the scenes it'll run the providers load method.
		/// </summary>
		/// <param name="onLoad">
		/// What to do when we finish loading both the scene and asset bundle
		/// </param>
		public void Load( Action onLoad = null )
		{
			if ( IsLoading )
			{
				throw new Exception( "Already performing an operation action this map" );
			}

			var lastMap = Current;
			lastMap?.Unload();

			onLoad += OnLoad;
			onLoad += () =>
			{
				Callback.Run( "map.loaded" );
			};

			Current = this;
			Callback.Run( "map.loading" );
			Provider.Load( onLoad );
		}

		/// <summary>
		/// Unload this Resource from memory.
		/// </summary>
		/// <param name="onUnload">
		/// What to do when we finish unloading
		/// </param>
		public void Unload( Action onUnload = null )
		{
			if ( IsLoading )
			{
				throw new Exception( "Already performing an operation action this map" );
			}

			// Add Callback
			onUnload += OnUnload;
			onUnload += () =>
			{
				Callback.Run( "map.unloaded" );
			};

			if ( Current == this )
			{
				Current = null;
			}

			Provider.Unload( onUnload );
		}

		/// <summary>
		/// Unload and remove it from the database.
		/// </summary>
		public void Dispose()
		{
			Unload( () => Database.Remove( this ) );
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

			Debugging.Log.Info( $"Map: [{Current.Title}] - [{Current.Identifier}]" );
		}

		[Debugging.Cmd( "map.load_path" )]
		private static void CmdLoadFromPath( string path )
		{
			Find( path, () => new Map( new AssetBundleMapProvider( new FileInfo( path ) ) ) )?.Load();
		}
	}
}
