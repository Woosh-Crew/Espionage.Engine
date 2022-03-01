using System;
using System.IO;
using Espionage.Engine.Components;
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
	[Group( "Maps" ), Path( "maps", "game://Maps/" )]
	public sealed class Map : Resource, ILoadable
	{
		public static Map Current { get; internal set; }

		// Provider
		public IProvider<Map, Scene> Provider { get; }
		public Components<Map> Components { get; }

		// Meta Data
		public override string Identifier => Provider.Identifier;
		public string Title { get; set; }
		public string Description { get; set; }

		// Loadable 
		float ILoadable.Progress => Provider.Progress;
		string ILoadable.Text => $"Loading {Title}";

		//
		// Constructors
		//

		/// <summary>Make a reference to a map, using a provider.</summary>
		/// <param name="provider">What provider should we use for loading and unloading maps?</param>
		public Map( IProvider<Map, Scene> provider )
		{
			Components = new Components<Map>( this );
			Provider = provider;
		}

		public Map( string path ) : this( new AssetBundleMapProvider( new FileInfo( path ) ) ) { }

		//
		// Resource 
		//

		public Action Loaded { get; set; }
		public Action Unloaded { get; set; }

		public override bool IsLoading => Provider.IsLoading;

		public override void Load( Action onLoad = null )
		{
			if ( IsLoading )
			{
				throw new Exception( "Already performing an operation action this map" );
			}

			base.Load( onLoad );

			onLoad += Loaded;
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

		public override void Unload( Action onUnload = null )
		{
			if ( IsLoading )
			{
				throw new Exception( "Already performing an operation action this map" );
			}

			base.Unload( onUnload );

			// Add Callback
			onUnload += Unloaded;
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
	}
}
