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
		public Vector3 WishVelocity { get; protected set; }
		private Vector3 _storedVelocity;

		public override void Simulate( Client client )
		{
			base.Simulate( client );

			var input = client.Input;
			var rot = transform.rotation;

			// Start Gravity
			Velocity -= new Vector3( 0, gravity / 2, 0 ) * Time.deltaTime;

			if ( Controller.isGrounded )
			{
				Velocity = Velocity.WithY( 0 );
			}

			// Smooth WishSpeed, so it isn't jarring
			WishSpeed = Mathf.Lerp( WishSpeed, GrabWishSpeed( client ), 8 * Time.deltaTime );

			WishVelocity = rot * Vector3.forward * input.Forward + rot * Vector3.right * input.Horizontal;
			WishVelocity = WishVelocity.normalized * (WishSpeed / 20) * Time.deltaTime;

			GroundedMove();

			Velocity = Vector3.SmoothDamp( Velocity, Vector3.zero, ref _storedVelocity, smoothing );

			// Finish Gravity
			Velocity -= new Vector3( 0, gravity / 2, 0 ) * Time.deltaTime;

			if ( Controller.isGrounded )
			{
				Velocity = Velocity.WithY( 0 );
			}
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

		public void Move()
		{
			Controller.Move( Velocity );
		}

		// Move

		protected virtual void GroundedMove()
		{
			Velocity += WishVelocity;
			Velocity = Vector3.ClampMagnitude( Velocity, WishSpeed / 100 );

			Move();
		}

		// Fields

		[SerializeField]
		private float walkSpeed = 15;

		[SerializeField]
		private float sprintSpeed = 15;

		[SerializeField]
		private float smoothing = 0.05f;

		[SerializeField]
		private float gravity = 20;
	}
}
