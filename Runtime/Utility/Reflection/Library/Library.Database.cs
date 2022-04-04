using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine
{
	public partial class Library
	{
		/// <summary>
		/// Database for Library Records. Allows the access of all records.
		/// Use extension methods to add functionality to database access.
		/// </summary>
		public static IDatabase<Library, string, Type> Database { get; private set; }

		private class InternalDatabase : IDatabase<Library, string, Type>
		{
			private readonly Dictionary<string, Library> _storage = new();

			public Library this[ string key ] => _storage[key];
			public Library this[ Type key ] => this.Get( key );

			// Enumerator

			public IEnumerator<Library> GetEnumerator()
			{
				// This shouldn't box. _store.GetEnumerator Does. but Enumerable.Empty shouldn't.
				return Count == 0 ? Enumerable.Empty<Library>().GetEnumerator() : _storage.Values.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			// API

			public void Add( Library item )
			{
				if ( string.IsNullOrEmpty( item.Title ) )
				{
					// Do space before uppercase char 
					var type = item.Info;

					// This is so aids...
					if ( type.IsGenericType )
					{
						item.Title = (type.IsInterface ? type.Name : string.Concat( type.Name!.Select( x => char.IsUpper( x ) ? " " + x : x.ToString() ) ).TrimStart( ' ' )).Split( '`' )[0] + $"<{type.GetGenericArguments()[0]?.Name}>";
					}
					else
					{
						item.Title = type.IsInterface ? type.Name : string.Concat( type.Name!.Select( x => char.IsUpper( x ) ? " " + x : x.ToString() ) ).TrimStart( ' ' );
					}
				}

				if ( string.IsNullOrEmpty( item.Group ) )
				{
					item.Group = item.Info.Namespace;
				}

				// Store it in Database
				if ( _storage.ContainsKey( item.Name! ) )
				{
					_storage[item.Name] = item;
				}
				else
				{
					_storage.Add( item.Name!, item );
				}
			}

			public void Clear()
			{
				_storage.Clear();
			}

			public bool Contains( Library item )
			{
				return _storage.ContainsKey( item.Name );
			}

			public void Remove( Library item )
			{
				_storage.Remove( item.Name );
			}

			public int Count => _storage.Count;
		}
	}
}
