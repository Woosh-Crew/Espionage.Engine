using UnityEngine;

namespace Espionage.Engine
{
	public class FirstPersonController : PawnController
	{
		protected override void OnAwake()
		{
			Controller = GetComponent<CharacterController>();
		}

		public CharacterController Controller { get; private set; }

		public override void Simulate( Client client )
		{
			base.Simulate( client );

			var input = client.Input;
			var rot = transform.rotation;

			// Smooth WishSpeed, so it isn't jarring
			WishSpeed = Mathf.Lerp( WishSpeed, GrabWishSpeed( client ), 6 * Time.deltaTime );

			var wishDir = rot * Vector3.forward * input.Forward + rot * Vector3.right * input.Horizontal;
			wishDir = wishDir.normalized * WishSpeed * Time.deltaTime;

			Velocity = wishDir;
			Accelerate( wishDir.normalized, wishDir.magnitude, 0, 10 );

			Controller.Move( Velocity );
		}

		// Wish Speed

		public float WishSpeed { get; protected set; }

		protected virtual float GrabWishSpeed( Client cl )
		{
			if ( Input.GetKey( KeyCode.LeftShift ) && Mathf.Abs( Controller.velocity.magnitude ) > 0 )
			{
				return sprintSpeed;
			}

			return walkSpeed;
		}

		// Helpers

		/// <summary>
		/// Add our wish direction and speed onto our velocity
		/// </summary>
		public virtual void Accelerate( Vector3 wishdir, float wishspeed, float speedLimit, float acceleration )
		{
			// This gets overridden because some games (CSPort) want to allow dead (observer) players
			// to be able to move around.
			// if ( !CanAccelerate() )
			//     return;

			if ( speedLimit > 0 && wishspeed > speedLimit )
			{
				wishspeed = speedLimit;
			}

			// See if we are changing direction a bit
			var currentspeed = Vector3.Dot( Velocity, wishdir );

			// Reduce wishspeed by the amount of veer.
			var addspeed = wishspeed - currentspeed;

			// If not going to add any speed, done.
			if ( addspeed <= 0 )
			{
				return;
			}

			// Determine amount of acceleration.
			var accelspeed = acceleration * Time.deltaTime * wishspeed;

			// Cap at addspeed
			if ( accelspeed > addspeed )
			{
				accelspeed = addspeed;
			}

			Velocity += wishdir * accelspeed;
		}

		// Fields

		[SerializeField]
		private float walkSpeed = 15;

		[SerializeField]
		private float sprintSpeed = 15;
	}
}
