using System;
using System.Collections.Generic;
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

		private void Unload( Action unloaded = null )
		{
			unloaded += OnUnload;

			if ( Current == this )
			{
				Current = null;
			}

			Provider.Unload();

			if ( Source == null )
			{
				unloaded.Invoke();
				return;
			}

			Source?.Unload( unloaded );
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

			if ( Current != null )
			{
				// Lazy Unload Operation
				queue.Enqueue( Operation.Create( Current.Unload, Current.Components.TryGet( out Meta meta ) ? $"Unloading {meta.Title}" : "Unloading Map" ) );
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

		public interface ICallbacks
		{
			void OnLoad( Scene scene );
			void OnUnload();

			ILoadable Inject() { return null; }
		}
	}
}
