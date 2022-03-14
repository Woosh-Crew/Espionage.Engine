using System;
using System.Collections.Generic;
using Espionage.Engine.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	/// <summary>
	/// Allows the loading and unloading of maps at runtime.
	/// You should be using this instead of UnityEngine.SceneManager.
	/// </summary>
	[Group( "Maps" ), Path( "maps", "assets://Maps/" )]
	public sealed class Map : IResource, ILibrary, ILoadable
	{
		public static Map Current { get; internal set; }

		[Function( "maps.init" ), Callback( "engine.ready" )]
		private static void Initialize()
		{
			Resource.IProvider<Map> provider = Application.isEditor ? new EditorSceneMapProvider() : new BuildIndexMapProvider( 0 );

			// Get main scene at start, that isn't engine layer.
			for ( var i = 0; i < SceneManager.sceneCount; i++ )
			{
				var scene = SceneManager.GetSceneAt( i );

				if ( scene.name != Engine.Scene.name )
				{
					SceneManager.SetActiveScene( scene );
					break;
				}
			}

			Current = new( provider );
		}

		/// <summary>
		/// Trys to find the map by path. If it couldn't find the map in the database,
		/// it'll create a new reference to that map for use later.
		/// </summary>
		public static Map Find( string path )
		{
			path = Files.Pathing.Absolute( path );

			// Use the Database Map if we have it
			if ( Database[path] != null )
			{
				return Database[path];
			}

			if ( !Files.Pathing.Exists( path ) )
			{
				Debugging.Log.Error( $"Map Path [{Files.Pathing.Absolute( path )}], couldn't be found." );
				return null;
			}

			var file = Files.Serialization.Load<IFile<Map>>( path );
			return new( file.Provider );
		}

		//
		// Instance
		//

		public Library ClassInfo { get; } = Library.Database[typeof( Map )];
		public string Identifier => Provider.Identifier;

		public Components<Map> Components { get; }
		private Resource.IProvider<Map> Provider { get; }

		// Loadable 
		float ILoadable.Progress => Provider.Progress;
		string ILoadable.Text => Components.TryGet( out Meta meta ) ? $"Loading {meta.Title}" : "Loading";

		private Map( Resource.IProvider<Map> provider )
		{
			Components = new( this );

			Provider = provider;
			Database.Add( this );
		}

		public void Load( Action loaded = null )
		{
			loaded += () => Callback.Run( "map.loaded" );

			Callback.Run( "map.loading" );

			// Unload first, then load the next map
			if ( Current != null )
			{
				Debugging.Log.Info( "Unloading, then loading" );
				Current?.Unload( () => Provider.Load( loaded ) );
			}
			else
			{
				Debugging.Log.Info( "Loading Map" );
				Provider.Load( loaded );
			}

			Current = this;
		}

		public void Unload( Action unloaded = null )
		{
			// Add Callback
			unloaded += () => Callback.Run( "map.unloaded" );

			if ( Current == this )
			{
				Current = null;
			}

			Provider.Unload( unloaded );
		}

		//
		// Database
		//

		private static IDatabase<Map, string> Database { get; } = new InternalDatabase();

		private class InternalDatabase : IDatabase<Map, string>
		{
			public IEnumerable<Map> All => _records.Values;
			public int Count => _records.Count;

			public Map this[ string key ] => _records.ContainsKey( key ) ? _records[key] : null;

			private readonly Dictionary<string, Map> _records = new();

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
		}
	}
}
