using System;
using System.Collections.Generic;
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
	[Group( "Maps" ), Path( "maps", "assets://Maps/" )]
	public sealed class Map : IResource, ILibrary, ILoadable
	{
		public static Map Current { get; internal set; }

		// Provider
		public Resource.IProvider<Map, Scene> Provider { get; }
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
		public Map( Resource.IProvider<Map, Scene> provider )
		{
			ClassInfo = Library.Register( this );

			Components = new Components<Map>( this );
			Provider = provider;

			Database.Add( this );
		}

		~Map()
		{
			Library.Unregister( this );
		}

		public static Map Find( string path )
		{
			return Database[path] ?? new Map( Files.Load<IFile<Map, Scene>>( path ).Provider() );
		}

		//
		// Resource 
		//

		public Action Loaded { get; set; }
		public Action Unloaded { get; set; }

		public bool IsLoading => Provider.IsLoading;

		public void Load( Action onLoad = null )
		{
			if ( IsLoading )
			{
				throw new Exception( "Already performing an operation action this map" );
			}

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

		public void Unload( Action onUnload = null )
		{
			if ( IsLoading )
			{
				throw new Exception( "Already performing an operation action this map" );
			}

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

		//
		// Database
		//

		/// <summary>
		/// A reference to all the maps that have already been found or loaded.
		/// </summary>
		public static IDatabase<Map, string> Database { get; } = new InternalDatabase();

		private class InternalDatabase : IDatabase<Map, string>
		{
			public IEnumerable<Map> All => _records.Values;
			private readonly Dictionary<string, Map> _records = new();

			public Map this[ string key ] => _records.ContainsKey( key ) ? _records[key] : null;

			public void Add( Map item )
			{
				// Store it in Database
				if ( _records.ContainsKey( item.Identifier! ) )
				{
					_records[item.Identifier] = item;
				}
				else
				{
					_records.Add( item.Identifier!, item );
				}
			}

			public void Clear()
			{
				_records.Clear();
			}

			public bool Contains( Map item )
			{
				return _records.ContainsKey( item.Identifier );
			}

			public void Remove( Map item )
			{
				_records.Remove( item.Identifier );
			}

			public int Count => _records.Count;
		}
	}
}
