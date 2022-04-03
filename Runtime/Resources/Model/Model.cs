using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	[Group( "Models" ), Path( "models", "assets://Models/" )]
	public sealed class Model : ILibrary, IResource
	{
		public static Model Grab( string path )
		{
			path = Files.Pathing.Absolute( path );

			// Use the Database Map if we have it
			if ( Database[path] != null )
			{
				var model = Database[path];
				return model;
			}

			if ( Files.Pathing.Exists( path ) )
			{
				var model = new Model( Files.Grab<File>( path, false ) );
				return model;
			}

			Dev.Log.Error( $"Model Path [{path}], couldn't be found." );
			return null;
		}

		//
		// Instance
		//

		public string Identifier { get; }
		public bool Persistant { get; set; }
		public Library ClassInfo { get; }

		private Model( File provider )
		{
			Assert.IsNull( provider );

			Source = provider;
			Identifier = provider.Info.FullName;
			ClassInfo = Library.Register( this );
		}

		public File Source { get; private set; }
		private GameObject Cached { get; set; }

		public int Instances { get; private set; }

		internal void Load( Action<GameObject> onLoad )
		{
			if ( Instances <= 0 )
			{
				onLoad += OnLoad;

				// Add to Database
				Database.Add( this );
				Source.Load( onLoad );
			}

			Instances++;
		}

		internal void OnLoad( GameObject gameObject )
		{
			Cached = gameObject;
			UnityEngine.Object.Instantiate( gameObject );
		}

		internal void Unload()
		{
			Instances--;

			if ( Instances <= 0 )
			{
				Source.Unload();
				Database.Remove( this );
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
					Dev.Log.Warning( $"Replacing Resource [{item.Identifier}]" );
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
