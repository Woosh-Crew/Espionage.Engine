using System;
using System.IO;
using Espionage.Engine.Components;

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
	public partial class Map : IResource, IDisposable, ILibrary, ILoadable
	{
		public static Map Current { get; internal set; }

		// Provider
		protected IMapProvider Provider { get; }
		public Components<Map> Components { get; }

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
			Components = new Components<Map>( this );

			Provider = provider;
			Database.Add( this );
		}

		public Map( string path ) : this( new AssetBundleMapProvider( new FileInfo( path ) ) ) { }

		/// <summary>
		/// Gets the map at a path.
		/// </summary>
		/// <param name="path">The path to this map.</param>
		/// <param name="noneFound">If there is no map found, what do we do?</param>
		public static Map Find( string path, Func<Map> noneFound = null )
		{
			if ( string.IsNullOrEmpty( path ) )
			{
				throw new FileNotFoundException( "Invalid File / Path" );
			}

			var map = Database[path] ?? (noneFound?.Invoke() ?? new Map( path ));
			return map;
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

			onLoad += OnLoad;
			onLoad += () =>
			{
				Callback.Run( "map.loaded" );
			};

			if ( Current == this )
			{
				onLoad?.Invoke();
				return;
			}

			var lastMap = Current;

			// Unload first, then load the next map
			if ( lastMap != null )
			{
				Debugging.Log.Info( "Unloading, then loading" );
				lastMap?.Unload( () => Provider.Load( onLoad ) );
			}
			else
			{
				Provider.Load( onLoad );
			}

			Current = this;
			Callback.Run( "map.loading" );
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
	}

	/// <inheritdoc cref="Map"/>
	/// <typeparam name="T"> Provider </typeparam>
	public class Map<T> : Map where T : class, IMapProvider
	{
		public new T Provider => base.Provider as T;
		public Map( T provider ) : base( provider ) { }
	}
}
