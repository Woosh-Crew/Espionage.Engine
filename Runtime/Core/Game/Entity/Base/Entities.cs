using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Entities is an Entity Manager, It is responsible for containing a
	/// reference to all entities that are currently in the game world. Use
	/// this for finding entities by type, name, etc.
	/// </summary>
	public sealed class Entities : Module, IEnumerable<Entity>
	{
		protected override bool OnRegister()
		{
			Entity.All = this;
			return true;
		}

		protected override void OnUpdate()
		{
			foreach ( var entity in Entity.All )
			{
				(entity ? entity : null)?.Frame();
				(entity ? entity : null)?.Thinking.Run();
			}
		}

		[Function, Callback( "map.loaded" )]
		public void OnMapLoaded()
		{
			foreach ( var proxy in GameObject.FindObjectsOfType<Proxy>() )
			{
				var ent = proxy.Create();
				if ( ent == null )
				{
					continue;
				}

				ent.Register( proxy.properties, proxy.outputs.Select( e =>
				{
					var split = e.Value.Split( ',' );
					return new Output( e.Key, split[0], split[1], 0 );
				} ).ToArray() );

				ent.Spawn();

				ent.Enabled = !proxy.disabled;
				ent.Name = proxy.name;

				ent.MoveTo( proxy.transform );
			}
		}

		// Database
		// --------------------------------------------------------------------------------------- //

		public const int Max = 2048;
		private readonly SortedList<int, Entity> _storage = new( Max );

		internal void Add( Entity item )
		{
			_storage.Add( item.Identifier, item );
		}

		internal void Remove( Entity item )
		{
			_storage.Remove( item.Identifier );
		}

		// Enumerator
		// --------------------------------------------------------------------------------------- //

		public IEnumerator<Entity> GetEnumerator()
		{
			return _storage.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		// Public API
		// --------------------------------------------------------------------------------------- //

		public Entity this[ int key ] => _storage.TryGetValue( key, out var value ) ? value : null;

		public IEnumerable<Entity> this[ string key, bool includeDisabled = false ]
		{
			get
			{
				if ( key.IsEmpty() )
				{
					return null;
				}

				// Find by Library
				if ( key.EndsWith( '*' ) )
				{
					key = key[..^1];
					return _storage.Values.Where( e => (includeDisabled || e.Enabled) && e.ClassInfo.Name.StartsWith( key ) );
				}

				// Find by Name
				return _storage.Values.Where( e => (includeDisabled || e.Enabled) && e.Name.Equals( key, StringComparison.CurrentCultureIgnoreCase ) );
			}
		}

		public T Find<T>() where T : Entity
		{
			return (T)_storage.Values.FirstOrDefault( e => e is T );
		}

		public T Find<T>( Library library ) where T : Entity
		{
			return (T)_storage.Values.FirstOrDefault( e => e.ClassInfo == library );
		}

		// Inside Sphere
		// --------------------------------------------------------------------------------------- //

		public T[] InSphere<T>( Vector3 position, float radius ) where T : Entity
		{
			return this.OfType<T>().Where( entity => (entity.Position - position).magnitude <= radius ).ToArray();
		}

		public Entity[] InSphere( Vector3 position, float radius )
		{
			return this.Where( entity => (entity.Position - position).magnitude <= radius ).ToArray();
		}

		// Inside Bounds
		// --------------------------------------------------------------------------------------- //

		public T[] InBox<T>( Vector3 position, Vector3 size ) where T : Entity
		{
			return InBounds<T>( new( position, size ) );
		}

		public Entity[] InBox( Vector3 position, Vector3 size )
		{
			return InBounds( new( position, size ) );
		}

		public T[] InBounds<T>( Bounds bounds ) where T : Entity
		{
			return this.OfType<T>().Where( entity => bounds.Contains( entity.Position ) ).ToArray();
		}

		public Entity[] InBounds( Bounds bounds )
		{
			return this.Where( entity => bounds.Contains( entity.Position ) ).ToArray();
		}

		// Inside Cone
		// --------------------------------------------------------------------------------------- //

		public T[] InCone<T>( Vector3 origin, Vector3 direction, float angle, float range ) where T : Entity
		{
			return this.OfType<T>().Where( entity => IsPointInsideCone( entity.Position, origin, direction, angle, range ) ).ToArray();
		}

		public Entity[] InCone( Vector3 origin, Vector3 direction, float angle, float range )
		{
			return this.Where( entity => IsPointInsideCone( entity.Position, origin, direction, angle, range ) ).ToArray();
		}

		private static bool IsPointInsideCone( Vector3 point, Vector3 origin, Vector3 dir, float maxAngle, float range )
		{
			var distance = (point - origin).magnitude;

			if ( distance > range )
			{
				return false;
			}

			var angle = Vector3.Angle( dir, point - origin );
			return angle < maxAngle;
		}
	}
}
