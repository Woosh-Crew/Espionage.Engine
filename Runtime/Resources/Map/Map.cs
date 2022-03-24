using System;
using System.Collections.Generic;
using Espionage.Engine.Components;
using UnityEngine;
using UnityEngine.SceneManagement;
using Espionage.Engine.Resources.Binders;

namespace Espionage.Engine.Resources
{
	/// <summary>
	/// Allows the loading and unloading of maps at runtime.
	/// You should be using this instead of UnityEngine.SceneManager.
	/// </summary>
	[Group( "Maps" ), Path( "maps", "assets://Maps/" )]
	public sealed partial class Map : IResource, ILibrary, ILoadable
	{
		public static Map Current { get; private set; }

		[Function( "maps.init" ), Callback( "engine.getting_ready" )]
		private static void Initialize()
		{
			Binder provider = Application.isEditor ? new EditorSceneMapProvider() : new BuildIndexMapProvider( 0 );

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
			Callback.Run( "map.loaded" );

			// Cache Maps

			for ( var i = 0; i < SceneManager.sceneCountInBuildSettings; i++ )
			{
				var scene = SceneManager.GetSceneByBuildIndex( i );
				Setup( new BuildIndexMapProvider( i ) ).Meta( scene.name ).Origin( "Game" ).Build();
			}

			foreach ( var item in Files.Pathing.All( "maps://" ) )
			{
				Setup( item ).Meta( Files.Pathing.Name( item ) ).Origin( "Game" ).Build();
			}
		}

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

		[Property] public string Identifier => Source != null ? Source.Info.FullName : Provider.Identifier;
		public Components<Map> Components { get; }

		private File Source { get; set; }
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

		private Map( File file )
		{
			Assert.IsNull( file );
			Components = new( this );

			Source = file;
			Database.Add( this );
		}

		private Map( Binder provider )
		{
			Assert.IsNull( provider );
			Components = new( this );

			Provider = provider;
			Database.Add( this );
		}

		public Action Loaded { get; set; }
		public Action Unloaded { get; set; }

		void ILoadable.Load( Action loaded )
		{
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
			Callback.Run( "map.loaded" );
			Loaded?.Invoke();

			SceneManager.SetActiveScene( Provider.Scene );
		}

		private void Unload( Action unloaded = null )
		{
			unloaded += () => Callback.Run( "map.unloaded" );
			unloaded += Unloaded;

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

		// Map Injection

		ILoadable[] ILoadable.Inject()
		{
			var queue = new Queue<ILoadable>();

			if ( Current != null )
			{
				// Lazy Unload Operation
				queue.Enqueue( Operation.Create( Current.Unload, Current.Components.TryGet( out Meta meta ) ? $"Unloading {meta.Title}" : "Unloading" ) );
			}

			if ( Source != null )
			{
				// Load data from source file
				queue.Enqueue( Source );
			}

			return queue.ToArray();
		}
	}
}
