using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Espionage.Engine.Resources
{
	[Library( "res.model" ), Group( "Models" ), Path( "models", "assets://<library_group(res.model)>/", Overridable = true )]
	public sealed class Model : ILibrary, IResource
	{
		public static Model Load( string path )
		{
			if ( !Files.Pathing.Valid( path ) )
			{
				path = "models://" + path;
			}

			path = Files.Pathing.Absolute( path );

			// Use the Database Map if we have it
			if ( Database[path] != null )
			{
				var model = Database[path];
				model.Load();
				return model;
			}

			if ( Files.Pathing.Exists( path ) )
			{
				var model = new Model( Files.Grab<File>( path ) );
				model.Load();
				return model;
			}

			// Either Load Error Model, or nothing if not found.
			Debugging.Log.Error( $"Model Path [{path}], couldn't be found." );
			return Files.Pathing.Exists( "models://w_error.umdl" ) ? Load( "w_error.umdl" ) : null;
		}

		//
		// Instance
		//

		public Library ClassInfo { get; }

		public string Identifier { get; }
		public bool Persistant { get; set; }

		private Model( File provider )
		{
			Assert.IsNull( provider );
			ClassInfo = Library.Register( this );

			Source = provider;
			Identifier = provider.Info.FullName;
		}

		public File Source { get; }
		public GameObject Cache { get; set; }
		private Stack<Instance> Instances { get; set; } = new();

		public Instance Consume( Transform transform )
		{
			var instance = Instances.Peek();

			if ( instance.IsConsumed )
			{
				Assert.IsTrue( instance.IsConsumed );
				return null;
			}

			instance.IsConsumed = true;
			instance.GameObject.SetActive( true );
			instance.GameObject.transform.parent = transform;

			return instance;
		}

		private void Load()
		{
			if ( Instances.Count <= 0 )
			{
				Database.Add( this );
				Source.Load( OnLoad );

				Debugging.Log.Info( $"Loaded Model [{Files.Pathing.Name( Identifier )}]" );
			}

			Instances.Push( new( this ) );
		}

		private void OnLoad( GameObject gameObject )
		{
			Cache = gameObject;
		}

		private void Unload()
		{
			Instances.Pop();

			if ( Instances.Count <= 0 )
			{
				Debugging.Log.Info( $"Unloading Model [{Files.Pathing.Name( Identifier )}]" );

				Source.Unload();
				Database.Remove( this );
				(this as ILibrary).Delete();

				Cache = null;
			}
		}

		public static implicit operator Model( string input )
		{
			return Load( input );
		}


		public sealed class Instance
		{
			public Instance( Model model )
			{
				Model = model;
				IsConsumed = false;

				GameObject = Object.Instantiate( Model.Cache );
				GameObject.SetActive( false );
			}

			public Model Model { get; }
			public GameObject GameObject { get; }
			public bool IsConsumed { get; set; }

			public void Delete()
			{
				Object.Destroy( GameObject );
				Model.Unload();
			}
		}

		[Library( "mdl.file" ), Group( "Models" )]
		public abstract class File : IFile
		{
			public Library ClassInfo { get; }

			protected File()
			{
				ClassInfo = Library.Register( this );
			}

			public FileInfo Info { get; set; }

			public abstract void Load( Action<GameObject> loaded );
			public abstract void Unload();
		}

		//
		// Database
		//

		public static IDatabase<Model, string> Database { get; } = new InternalDatabase();

		private class InternalDatabase : IDatabase<Model, string>
		{
			private readonly Dictionary<string, Model> _storage = new();

			public Model this[ string key ] => _storage.ContainsKey( key ) ? _storage[key] : null;
			public int Count => _storage.Count;

			// Enumerator

			public IEnumerator<Model> GetEnumerator()
			{
				// This shouldn't box. _store.GetEnumerator Does. but Enumerable.Empty shouldn't.
				return Count == 0 ? Enumerable.Empty<Model>().GetEnumerator() : _storage.Values.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			// API

			public void Add( Model item )
			{
				// Store it in Database
				if ( _storage.ContainsKey( item.Identifier! ) )
				{
					Debugging.Log.Warning( $"Replacing Resource [{item.Identifier}]" );
					_storage[item.Identifier] = item;
				}
				else
				{
					_storage.Add( item.Identifier!, item );
				}
			}

			public bool Contains( Model item )
			{
				return _storage.ContainsKey( item.Identifier );
			}

			public void Remove( Model item )
			{
				_storage.Remove( item.Identifier );
			}

			public void Clear()
			{
				_storage.Clear();
			}
		}
	}
}
