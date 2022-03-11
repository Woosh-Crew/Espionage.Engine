using System;
using UnityEngine;

namespace Espionage.Engine
{
	public class FirstPersonController : Pawn.Controller
	{
		private CharacterController _characterController;

		protected override void OnAttached( Pawn item )
		{
			if ( !TryGetComponent( out _characterController ) )
			{
				_characterController = gameObject.AddComponent<CharacterController>();
				_characterController.height = 1.8f;
				_characterController.center = _characterController.center.WithY( 1.8f / 2 );
			}
		}

		protected override void Simulate()
		{
			// player movement - forward, backward, left, right
			var input = new Vector2( Client.Input.Horizontal, Client.Input.Forward );
			input = input.normalized;
			input *= moveSpeed;

			var wishDirection = transform.rotation * (Vector3.right * input.x + Vector3.forward * input.y) * Time.deltaTime;

			Velocity -= Gravity( gravity / 2 );

			if ( _characterController.isGrounded )
			{
				Velocity += Accelerate( wishDirection, moveSpeed, 0.7f );
				Velocity *= Decelerate( 8, 5f );
			}

			_characterController.Move( Velocity );
		}

		public Vector3 Accelerate( Vector3 wishDirection, float wishSpeed, float cap )
		{
			if ( cap > 0 && wishSpeed > cap )
			{
				wishSpeed = cap;
			}

			// See if we are changing direction a bit
			var currentSpeed = Velocity.Dot( wishDirection );

			currentSpeed = Mathf.Clamp( currentSpeed, 0, cap );

			// Reduce wishSpeed by the amount of veer.
			var addSpeed = wishSpeed - currentSpeed;

			// If not going to add any speed, done.
			if ( addSpeed <= 0 )
			{
				return Vector3.zero;
			}

			// Determine amount of acceleration.
			var accelSpeed = 5 * Time.deltaTime * wishSpeed;

			// Cap at addSpeed
			if ( accelSpeed > addSpeed )
			{
				accelSpeed = addSpeed;
			}

			return wishDirection * accelSpeed;
		}

		public float Decelerate( float friction, float stopSpeed )
		{
			var speed = Velocity.magnitude;

			if ( speed < 0.1f )
			{
				return 1;
			}

			var control = speed < stopSpeed ? stopSpeed : speed;
			var drop = control * Time.deltaTime * friction;

			var newSpeed = speed - drop;
			if ( newSpeed < 0 )
			{
				newSpeed = 0;
			}

			if ( Math.Abs( newSpeed - speed ) > 0.002f )
			{
				newSpeed /= speed;
				return newSpeed;
			}

			return 1;
		}

		public Vector3 Gravity( float amount )
		{
			return new( 0, Velocity.y + amount, 0 );
		}

		// Fields

		[SerializeField]
		private float moveSpeed = 7;

		[SerializeField]
		private float gravity = 9.8f;
	}
}
