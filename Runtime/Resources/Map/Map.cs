using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Espionage.Engine.Components;
using Espionage.Engine.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	[Group( "Maps" ), Path( "maps", "assets://Maps/" )]
	public sealed partial class Map : ILibrary, ILoadable
	{
		public static Map Current { get; internal set; }
		public static string[] Extensions { get; } = Library.Database.GetAll<File>().Select( e => e.Components.Get<FileAttribute>()?.Extension ).ToArray();

		/// <summary>
		/// Trys to find the map by path. If it couldn't find the map in the database,
		/// it'll create a new reference to that map for use later if it exists.
		/// </summary>
		public static Map Find( Pathing path )
		{
			if ( !path.Valid() )
			{
				path = "maps://" + path;
			}

			path = path.Absolute();

			// Use the Database Map if we have it
			if ( Database[path] != null )
			{
				return Database[path];
			}

			if ( path.Exists() )
			{
				return new( Files.Grab<File>( path ) );
			}

			Debugging.Log.Error( $"Map Path [{path}], couldn't be found." );
			return null;
		}

		/// <summary>
		/// Checks if the map / path already exists in the maps database. 
		/// </summary>
		public static bool Exists( Pathing path )
		{
			return Database[path.Absolute()] != null;
		}

		//
		// Instance
		//

		public string Identifier { get; }
		public Components<Map> Components { get; }

		private File Source { get; }
		private Binder Provider { get; set; }

		// Loadable 

		[Title( "Progress" )]
		float ILoadable.Progress => Provider?.Progress ?? 0;

		[Title( "Text" )]
		string ILoadable.Text => Components.TryGet( out Meta meta ) ? $"Loading {meta.Title}" : "Loading";

		// Class

		public Library ClassInfo { get; } = Library.Database[typeof( Map )];

		private Map( File file )
		{
			Assert.IsNull( file );
			Components = new( this );

			Source = file;

			Identifier = Source.Info.FullName;
			Database.Add( this );
		}

		internal Map( Binder provider, string id )
		{
			Assert.IsNull( provider );

			Components = new( this );
			Provider = provider;

			Identifier = id;
			Database.Add( this );
		}

		void ILoadable.Load( Action loaded )
		{
			Assert.Missing( Database, this );

			loaded += OnLoad;

			Callback.Run( "map.loading" );

			// Get provider from source file
			if ( Source != null )
			{
				Provider = Source.Binder;
			}

			Provider.Load( loaded );
			Current = this;
		}

		private void OnLoad()
		{
			void PissOff<T>( GameObject root ) where T : UnityEngine.Behaviour
			{
				foreach ( var component in root.GetComponentsInChildren<T>() )
				{
					component.enabled = false;
				}
			}

			SceneManager.SetActiveScene( Provider.Scene );

			foreach ( var comp in Components.GetAll<ICallbacks>() )
			{
				comp.OnLoad( Provider.Scene );
			}

			// Piss off all the crap we don't need
			foreach ( var gameObject in Provider.Scene.GetRootGameObjects() )
			{
				PissOff<Camera>( gameObject );
				PissOff<AudioListener>( gameObject );
			}

			Callback.Run( "map.loaded" );
		}

		private void OnUnload()
		{
			Callback.Run( "map.unloaded" );

			foreach ( var comp in Components.GetAll<ICallbacks>() )
			{
				comp.OnUnload();
			}
		}

		// Map Injection

		ILoadable[] ILoadable.Inject()
		{
			var queue = new Queue<ILoadable>();

			// Unload Current Map
			if ( Current != null )
			{
				// Lazy Unload Operation
				if ( Current.Provider != null )
				{
					queue.Enqueue( Operation.Create( Current.Provider.Unload, "Unloading Scene" ) );
				}

				if ( Current.Source != null )
				{
					queue.Enqueue( Operation.Create( Current.Source.Unload, $"Unloading File [{Current.Source.Info.Name}]" ) );
				}

				Current.OnUnload();

				if ( Current == this )
				{
					Current = null;
				}
			}

			// Inject in components, if they have injections
			foreach ( var comp in Components.GetAll<ICallbacks>() )
			{
				var inject = comp.Inject();
				if ( inject != null )
				{
					queue.Enqueue( inject );
				}
			}

			if ( Source != null )
			{
				// Load data from source file
				queue.Enqueue( Source );
			}

			return queue.ToArray();
		}

		//
		// Components
		//

		public interface ICallbacks
		{
			void OnLoad( Scene scene );
			void OnUnload();

			ILoadable Inject() { return null; }
		}

		//
		// Binder
		//

		public abstract class Binder
		{
			public Scene Scene { get; protected set; }
			public virtual float Progress { get; protected set; }

			public abstract void Load( Action onLoad );

			public virtual void Unload( Action finished )
			{
				if ( Scene == default )
				{
					return;
				}

				Callback.Run( "map.unloading" );
				Scene.Unload().completed += _ => finished.Invoke();
				Scene = default;
			}
		}

		[Library( "map.file" ), Group( "Maps" )]
		public abstract class File : IFile, ILoadable
		{
			public Binder Binder { get; protected set; }

			public Library ClassInfo { get; }

			public File()
			{
				ClassInfo = Library.Register( this );
			}

			public FileInfo Info { get; set; }

			public abstract void Load( Action loaded );
			public abstract void Unload( Action finished );

			// ILoadable

			public virtual float Progress { get; protected set; }
			public virtual string Text => $"Loading File [{Info.Name}]";
		}

		//
		// Commands
		//

		[Function( "map" ), Terminal]
		private static void GrabOrLaunchMap( string path = null )
		{
			if ( path.IsEmpty() )
			{
				Debugging.Log.Info( $"Map: {Current.Identifier}" );
				return;
			}

			// Load new Map
			Engine.Game.Loader.Start(
				Find( path )
			);
		}

		public static implicit operator Map( string input )
		{
			return Find( input );
		}

		//
		// Database
		//

		public static IDatabase<Map, string> Database { get; } = new InternalDatabase();

		private class InternalDatabase : IDatabase<Map, string>
		{
			// IDatabase

			public int Count => _records.Count;
			public Map this[ string key ] => _records.ContainsKey( key ) ? _records[key] : null;

			// Instance

			private readonly Dictionary<string, Map> _records = new( StringComparer.CurrentCultureIgnoreCase );

			// Enumerator

			public IEnumerator<Map> GetEnumerator()
			{
				// This shouldn't box. _store.GetEnumerator Does. but Enumerable.Empty shouldn't.
				return Count == 0 ? Enumerable.Empty<Map>().GetEnumerator() : _records.Values.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			// API

			public void Add( Map item )
			{
				// Store it in Database
				if ( _records.ContainsKey( item.Identifier! ) )
				{
					Debugging.Log.Warning( $"Replacing Map [{item.Identifier}]" );
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
