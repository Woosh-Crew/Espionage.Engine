using UnityEngine;

namespace Espionage.Engine
{
	public static class Trace
	{
		public static Builder Ray( Vector3 origin, Vector3 direction, float distance )
		{
			return new( origin, direction, distance );
		}

		// Builder

		public struct Builder
		{
			private readonly Vector3 _origin;
			private readonly Vector3 _direction;
			private float _distance;
			private float _radius;

			public Builder( Vector3 origin, Vector3 direction, float distance )
			{
				_origin = origin;
				_direction = direction;
				_distance = distance;

				_radius = 0;
				_ignore = null;
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
					? Physics.Raycast( _origin, _direction, out test, _distance, LayerMask.GetMask( _ignore ), QueryTriggerInteraction.Ignore )
					: Physics.SphereCast( _origin, _radius, _direction, out test, _distance, LayerMask.GetMask( _ignore ), QueryTriggerInteraction.Ignore );

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
				if ( !result )
				{
					hit = null;
					return null;
				}

				// Try get a component first
				if ( test.HasValue && test.Value.collider.TryGetComponent<T>( out var item ) )
				{
					hit = test;
					return item;
				}

				// Try an Entity
				if ( test.HasValue && test.Value.collider.TryGetComponent<Entity>( out var entity ) )
				{
					hit = test;
					return entity.Get<T>();
				}

				hit = null;
				return null;
			}
		}
	}
}
