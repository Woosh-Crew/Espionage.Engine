using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine
{
	/// <summary>
	/// Entities is not an Entity Manager, It is responsible for containing a
	/// reference to all entities that are currently in the game world. Use
	/// this for finding entities by type, name, etc.
	/// </summary>
	public sealed class Entities : IEnumerable<Entity>
	{
		public int Count => _storage.Count;

		private readonly List<Entity> _storage = new();

		// Enumerator

		public IEnumerator<Entity> GetEnumerator()
		{
			return _storage.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		// Entity Access

		internal void Add( Entity item )
		{
			_storage.Add( item );
		}

		internal void Remove( Entity item )
		{
			_storage.Remove( item );
		}

		// API

		public Entity this[ int key ] => _storage[key];

		public Entity this[ string key ]
		{
			get
			{
				if ( string.IsNullOrWhiteSpace( key ) )
				{
					return null;
				}

				return _storage.FirstOrDefault( e => string.Equals( e.Identifier, key, StringComparison.CurrentCultureIgnoreCase ) );
			}
		}

		public T Find<T>() where T : Entity
		{
			return _storage.FirstOrDefault( e => e is T ) as T;
		}

		public IEnumerable<Entity> Find( string id )
		{
			return _storage.Where( e => string.Equals( e.Identifier, id, StringComparison.CurrentCultureIgnoreCase ) );
		}
	}
}
