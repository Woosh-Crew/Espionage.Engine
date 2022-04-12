using System;
using System.Collections;
using System.Collections.Generic;

namespace Espionage.Engine
{
	public partial class Library
	{
		/// <summary>
		/// Database for Library Records. Allows the access of all records.
		/// Use extension methods to add functionality to database access.
		/// </summary>
		public static IDatabase<Library, string, Type, int> Database { get; private set; }

		private class InternalDatabase : IDatabase<Library, string, Type, int>
		{
			private readonly SortedList<int, Library> _storage = new();

			public int Count => _storage.Count;
			
			public Library this[ int hash ] => _storage[hash];
			public Library this[ string key ] => _storage[key.Hash()];
			public Library this[ Type key ] => this.Get( key );

			// Enumerator

			public IEnumerator<Library> GetEnumerator()
			{
				// This shouldn't box. _store.GetEnumerator Does. but Enumerable.Empty shouldn't.
				return _storage.Values.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			// API

			public void Add( Library item )
			{
				var hashedName = item.Name!.Hash();

				// Store it in Database
				if ( _storage.ContainsKey( hashedName ) )
				{
					_storage[hashedName] = item;
					Debugging.Log.Warning($"Replacing Library [{item.Name}]");
				}
				else
				{
					_storage.Add( hashedName, item );
				}
			}

			public void Clear()
			{
				_storage.Clear();
			}

			public bool Contains( Library item )
			{
				return _storage.ContainsKey( item.Name.Hash() );
			}

			public void Remove( Library item )
			{
				_storage.Remove( item.Name.Hash() );
			}
		}
	}
}
