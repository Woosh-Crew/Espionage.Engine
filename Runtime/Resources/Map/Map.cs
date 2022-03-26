using System;
using System.Collections.Generic;
using System.IO;
using Espionage.Engine.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	[Group( "Maps" ), Path( "maps", "assets://Maps/" )]
	public sealed partial class Map : IResource, ILibrary, ILoadable
	{
		public static Map Current { get; internal set; }

		/// <summary>
		/// Trys to find the map by path. If it couldn't find the map in the database,
		/// it'll create a new reference to that map for use later if it exists.
		/// </summary>
		public static Map Find( string path )
		{
			path = Files.Pathing.Absolute( path );

			// Use the Database Map if we have it
			if ( Database[path] != null )
			{
				return Database[path];
			}

			if ( Files.Pathing.Exists( path ) )
			{
				return new( Files.Grab<File>( path, false ) );
			}

			Dev.Log.Error( $"Map Path [{Files.Pathing.Absolute( path )}], couldn't be found." );
			return null;

		}

		/// <summary>
		/// Checks if the map / path already exists in the maps database. 
		/// </summary>
		public static bool Exists( string path )
		{
			path = Files.Pathing.Absolute( path );
			return Database[path] != null;
		}

		//
		// Instance
		//

		[Property] public string Identifier { get; }
		public Components<Map> Components { get; }

		private File Source { get; }
		private Binder Provider { get; set; }

		// Loadable 

		[Property] float ILoadable.Progress => Provider?.Progress ?? 0;

		[Property] string ILoadable.Text
		{
			get
			{
				if ( !string.IsNullOrWhiteSpace( Provider?.Text ) )
				{
					return Provider.Text;
				}

				return Components.TryGet( out Meta meta ) ? $"Loading {meta.Title}" : "Loading";
			}
		}

		// Class

		public Library ClassInfo { get; } = Library.Database[typeof( Map )];

		internal Map( File file )
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

			Callback.Run( "map.loaded" );
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
			public virtual string Text { get; protected set; }
			public virtual float Progress { get; protected set; }

			public Scene Scene { get; protected set; }

			public abstract void Load( Action onLoad );

			public virtual void Unload( Action finished )
			{
				Scene.Unload().completed += _ => finished.Invoke();
				Scene = default;
			}
		}

		[Group( "Maps" )]
		public abstract class File : IFile, IAsset, ILoadable
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
		// Database
		//

		public static IDatabase<Map, string> Database { get; } = new InternalDatabase();

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
