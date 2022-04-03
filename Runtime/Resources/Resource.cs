using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine.Resources
{
	public abstract class Resource<T> : ILibrary, IResource where T : class
	{
		public Library ClassInfo { get; }

		public Resource()
		{
			ClassInfo = Library.Register( this );
		}

		public string Identifier { get; protected set; }

		protected abstract void Load( Action<T> onLoad );
		protected abstract void Unload();

		//
		// Database
		//

		public static IDatabase<Resource<T>, string> Database { get; } = new InternalDatabase();

		private class InternalDatabase : IDatabase<Resource<T>, string>
		{
			private readonly Dictionary<string, Resource<T>> _storage = new();

			public Resource<T> this[ string key ] => _storage.ContainsKey( key ) ? _storage[key] : null;
			public int Count => _storage.Count;

			// Enumerator

			public IEnumerator<Resource<T>> GetEnumerator()
			{
				// This shouldn't box. _store.GetEnumerator Does. but Enumerable.Empty shouldn't.
				return Count == 0 ? Enumerable.Empty<Resource<T>>().GetEnumerator() : _storage.Values.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			// API

			public void Add( Resource<T> item )
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

			public bool Contains( Resource<T> item )
			{
				return _storage.ContainsKey( item.Identifier );
			}

			public void Remove( Resource<T> item )
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
