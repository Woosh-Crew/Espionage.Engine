using UnityEngine;

namespace Espionage.Engine
{
	public static class Trace
	{
		public static Builder Ray( Vector3 origin, Vector3 direction, float distance )
		{
			return new();
		}

		// Builder

		public struct Builder
		{
			private Vector3 _origin;
			private Vector3 _direction;
			private float _distance;
			private float _radius;
			
			// Sphere

			public Builder Radius( float value )
			{
				_radius = value;
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
				return _radius > 0 ? 
					Physics.Raycast( _origin, _direction, _distance, LayerMask.GetMask( _ignore ), QueryTriggerInteraction.Ignore ) :
					Physics.SphereCast(_origin, _radius, _direction, out _, _distance, LayerMask.GetMask(_ignore), QueryTriggerInteraction.Ignore);
			}
			
			public bool Run( out RaycastHit? hit )
			{
				RaycastHit test;
				
				var cast = _radius > 0 ? 
					Physics.Raycast( _origin, _direction, out test, _distance, LayerMask.GetMask( _ignore ), QueryTriggerInteraction.Ignore ) :
					Physics.SphereCast(_origin, _radius, _direction, out test, _distance, LayerMask.GetMask(_ignore), QueryTriggerInteraction.Ignore);

				if ( !cast )
				{
					hit = null;
					return false;
				}

				hit = test;
				return true;
			}
			
			public T Run<T>( out RaycastHit? hit ) where T : class
			{
				RaycastHit test;

				var cast = _radius > 0 ? 
					Physics.Raycast( _origin, _direction, out test, _distance, LayerMask.GetMask( _ignore ), QueryTriggerInteraction.Ignore ) :
					Physics.SphereCast(_origin, _radius, _direction, out test, _distance, LayerMask.GetMask(_ignore), QueryTriggerInteraction.Ignore);

				if ( !cast )
				{
					hit = null;
					return null;
				}

				hit = test;
				return test.collider.TryGetComponent<T>( out var item ) ? item : null;

			}
		}
	}
}
