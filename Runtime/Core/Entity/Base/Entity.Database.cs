using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

		private const int Max = 2048;
		private readonly List<Entity> _storage = new( Max );

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
		public Entity[] this[ string key ] => string.IsNullOrWhiteSpace( key ) ? null : _storage.Where( e => string.Equals( e.Identifier, key, StringComparison.CurrentCultureIgnoreCase ) ).ToArray();

		public T Find<T>() where T : Entity
		{
			return _storage.FirstOrDefault( e => e is T ) as T;
		}

		public T[] InSphere<T>( Vector3 position, float radius ) where T : Entity
		{
			return this.OfType<T>().Where( entity => (entity.Position - position).magnitude <= radius ).ToArray();
		}

		public Entity[] InSphere( Vector3 position, float radius )
		{
			return this.Where( entity => (entity.Position - position).magnitude <= radius ).ToArray();
		}

		public T[] InBounds<T>( Bounds bounds ) where T : Entity
		{
			return this.OfType<T>().Where( entity => bounds.Contains( entity.Position ) ).ToArray();
		}

		public Entity[] InBounds( Bounds bounds )
		{
			return this.Where( entity => bounds.Contains( entity.Position ) ).ToArray();
		}
	}
}
