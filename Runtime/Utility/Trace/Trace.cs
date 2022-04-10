using System;
using UnityEngine;

namespace Espionage.Engine
{
	public static class Trace
	{
		public static Builder Ray( Vector3 start, Vector3 end )
		{
			return new( start, (start - end).normalized, (start - end).magnitude );
		}

		public static Builder Ray( Vector3 origin, Vector3 direction, float distance )
		{
			return new( origin, direction, distance );
		}

		// Builder

		public struct Builder
		{
			private Vector3 _origin;
			private Vector3 _direction;
			private float _distance;
			private float _radius;

			public Builder( Vector3 origin, Vector3 direction, float distance )
			{
				_origin = origin;
				_direction = direction;
				_distance = distance;

				_radius = 0;
				_ignore = Array.Empty<string>();
			}

			public Builder Start( Vector3 value )
			{
				_origin = value;
				return this;
			}

			public Builder End( Vector3 value )
			{
				_direction = (_origin - value).normalized;
				_distance = (_origin - value).magnitude;
				return this;
			}

			public Builder Direction( Vector3 value )
			{
				_direction = value;
				return this;
			}

			// Sphere

			public Builder Radius( float value )
			{
				_radius = value;
				return this;
			}

			// Distance

			public Builder Distance( float value )
			{
				_distance = value;
				return this;
			}

			// Ignore

			private string[] _ignore;

			public Builder Ignore( string value )
			{
				_ignore = new[] { value };
				return this;
			}

			public Builder Ignore( params string[] value )
			{
				_ignore = value;
				return this;
			}

			// Run

			public bool Run()
			{
				return Run( out _ );
			}

			public bool Run( out RaycastHit? hit )
			{
				RaycastHit test;

				var cast = _radius > 0
					? Physics.Raycast( _origin, _direction, out test, _distance, ~LayerMask.GetMask( _ignore ), QueryTriggerInteraction.Ignore )
					: Physics.SphereCast( _origin, _radius, _direction, out test, _distance, ~LayerMask.GetMask( _ignore ), QueryTriggerInteraction.Ignore );

				if ( !cast )
				{
					hit = null;
					return false;
				}

				hit = test;
				return true;
			}

			public T Run<T>() where T : class
			{
				return Run<T>( out _ );
			}

			public T Run<T>( out RaycastHit? hit ) where T : class
			{
				var result = Run( out var test );

				// Didn't hit
				if ( !result || !test.HasValue )
				{
					hit = null;
					return null;
				}

				// T is Entity, do logic for Entity.
				if ( typeof( T ).IsAssignableFrom( typeof( Entity ) ) )
				{
					// Try an Entity
					if ( test.Value.collider.TryGetComponent<Entity>( out var entity ) )
					{
						hit = test;
						return entity.Get<T>();
					}

					hit = null;
					return null;
				}

				if ( test.Value.collider.TryGetComponent<T>( out var item ) )
				{
					hit = test;
					return item;
				}

				hit = null;
				return null;
			}
		}
	}
}
