using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine.Resources
{
	public partial class Resource
	{
		public static IDatabase<IResource, string> Database { get; } = new InternalDatabase();

		private class InternalDatabase : IDatabase<IResource, string, int>
		{
			private readonly SortedList<int, IResource> _storage = new();

			public IResource this[ string key ]
			{
				get
				{
					var hash = key.Hash();
					return _storage.ContainsKey( hash ) ? _storage[hash] : null;
				}
			}

			public IResource this[ int key ] => _storage.ContainsKey( key ) ? _storage[key] : null;
			public int Count => _storage.Count;

			// Enumerator

			public IEnumerator<IResource> GetEnumerator()
			{
				// This shouldn't box. _store.GetEnumerator Does. but Enumerable.Empty shouldn't.
				return Count == 0 ? Enumerable.Empty<Model>().GetEnumerator() : _storage.Values.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			// API

			public void Add( IResource item )
			{
				// Store it in Database
				if ( _storage.ContainsKey( item.Identifier ) )
				{
					Debugging.Log.Warning( $"Replacing Resource [{item.Identifier}]" );
					_storage[item.Identifier] = item;
				}
				else
				{
					_storage.Add( item.Identifier, item );
				}
			}

			public bool Contains( IResource item )
			{
				return _storage.ContainsKey( item.Identifier );
			}

			public void Remove( IResource item )
			{
				_storage.Remove( item.Identifier );
				item.Delete();
			}

			public void Clear()
			{
				_storage.Clear();
			}
		}
	}
}
