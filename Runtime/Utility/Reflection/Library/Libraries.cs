using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Espionage.Engine
{
	/// <summary>
	/// Libraries is used for handling every library instance. It is
	/// also responsible for caching libraries too. Its in its own
	/// class so we can decouple it, plus provide a wrapper / our own
	/// accessors to the database.
	/// </summary>
	public class Libraries : IEnumerable<Library>
	{
		private readonly SortedList<int, Library> _storage = new();

		public Library this[ int hash ]
		{
			get
			{
				try
				{
					return _storage[hash];
				}
				catch ( KeyNotFoundException )
				{
					Debugging.Log.Error( $"ClassName ID [{hash}], was not found in Library Database" );
					return null;
				}
			}
		}

		public Library this[ string key ]
		{
			get
			{
				try
				{
					return _storage[key.Hash()];
				}
				catch ( KeyNotFoundException )
				{
					Debugging.Log.Error( $"ClassName {key}, was not found in Library Database" );
					return null;
				}
			}
		}

		public Library this[ Type key ] => _storage.Values.FirstOrDefault( e => e.Info == key );

		// API

		internal void Add( Library item )
		{
			var hashedName = item.Name!.Hash();

			// Store it in Database
			if ( _storage.ContainsKey( hashedName ) )
			{
				Debugging.Log.Warning( $"Replacing Library [{item.Name}]" );
				_storage[hashedName] = item;
				return;
			}

			_storage.Add( hashedName, item );
		}

		internal void Add( Type type )
		{
			if ( !type.IsDefined( typeof( LibraryAttribute ), false ) )
			{
				Add( new Library( type ) );
				return;
			}

			// If we have meta present, use it
			var attribute = type.GetCustomAttribute<LibraryAttribute>();
			Add( attribute.CreateRecord( type ) );
		}

		internal void Add( Assembly assembly )
		{
			foreach ( var type in assembly.GetTypes() )
			{
				if ( Library.IsValid( type ) )
				{
					Add( type );
				}
			}
		}

		internal void Add( AppDomain domain )
		{
			var main = typeof( Library ).Assembly;
			Add( main );

			foreach ( var assembly in AppDomain.CurrentDomain.GetAssemblies().Where( e => e.IsDefined( typeof( LibraryAttribute ) ) ) )
			{
				if ( assembly != main )
				{
					Add( assembly );
				}
			}
		}

		// Enumerator

		public IEnumerator<Library> GetEnumerator()
		{
			return _storage.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
