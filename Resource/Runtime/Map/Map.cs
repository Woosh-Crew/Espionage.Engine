using System;
using System.Collections.Generic;
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
	[Title( "Map" ), Group( "Maps" ), File( Extension = "map" )]
	public sealed partial class Map : IResource, IDisposable, ILibrary
	{
		/// <summary>
		/// The Current map that is loaded and running.
		/// </summary>
		public static Map Current { get; internal set; }

		/// <summary><inheritdoc cref="ILibrary.ClassInfo"/></summary>
		public Library ClassInfo { get; }

		/// <summary>
		/// Any other meta data that is attached to this class.
		/// Such as a SteamUGC reference, Icon, etc.
		/// </summary>
		public ComponentDatabase<Map> Components { get; }

		//
		// Meta Data
		//

		private IMapProvider Provider { get; }

		public string Identifier => Provider.Identifier;
		public string Title { get; set; }
		public string Description { get; set; }

		//
		// Constructors
		//

		/// <summary>Make a reference to a map, using a provider.</summary>
		/// <param name="provider">What provider should we use for loading and unloading maps?</param>
		public Map( IMapProvider provider )
		{
			ClassInfo = Library.Database[GetType()];
			Components = new ComponentDatabase<Map>( this );

			Provider = provider;
			Database.Add( this );
		}

		/// <summary>Make a map reference from a path.</summary>
		/// <param name="path">Where is the map located?</param>
		public Map( string path ) : this( new AssetBundleMapProvider( path ) ) { }

		/// <summary>
		/// Make a map reference from a build index.
		/// This shouldn't be used but it is there if you need it.
		/// </summary>
		/// <param name="buildIndex">Build Index of the scene in Build Scenes list</param>
		public Map( int buildIndex ) : this( new BuildIndexMapProvider( buildIndex ) ) { }

		/// <summary>
		/// Gets the map at a build index.
		/// </summary>
		/// <param name="index">The build index.</param>
		/// <returns>The found map, if applicable.</returns>
		public static Map Find( int index )
		{
			return Database[$"index:{index}"] ?? new Map( index );
		}

		/// <summary>
		/// Gets the map at a path.
		/// </summary>
		/// <param name="path">The path to this map.</param>
		/// <returns><inheritdoc cref="Find(int)"/></returns>
		public static Map Find( string path )
		{
			return Database[path] ?? new Map( path );
		}

		//
		// Operators
		//

		public static implicit operator Scene( Map map )
		{
			return map.Provider.Scene ?? default;
		}

		public static implicit operator Map( string path )
		{
			return Find( path );
		}

		public static implicit operator Map( int index )
		{
			return Find( index );
		}

		//
		// Utility
		//

		/// <summary>
		/// Add a GameObject to this map through code.
		/// </summary>
		/// <param name="gameObject">The GameObject to add</param>
		public void Add( GameObject gameObject )
		{
			if ( Provider.Scene != null )
			{
				SceneManager.MoveGameObjectToScene( gameObject, Provider.Scene.Value );
			}
		}

		//
		// Resource 
		//

		public bool IsLoading => Provider.IsLoading;

		/// <summary>
		/// Loads this map. Behind the scenes it'll run the providers load method.
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
			onLoad += () =>
			{
				Callback.Run( "map.loaded" );
			};

			Provider.Load( onLoad );
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

			if ( Current == this )
			{
				Current = null;
			}

			Internal_UnloadRequest( onUnload );

			return true;
		}

		private void Internal_UnloadRequest( Action onUnload = null )
		{
			onUnload += () =>
			{
				Callback.Run( "map.unloaded" );
			};

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

		[Debugging.Cmd( "map.load_index" )]
		private static void CmdLoadFromPath( int index )
		{
			Find( index )?.Load();
		}

		[Debugging.Cmd( "map.load_path" )]
		private static void CmdLoadFromPath( string path )
		{
			Find( path )?.Load();
		}
	}
}
