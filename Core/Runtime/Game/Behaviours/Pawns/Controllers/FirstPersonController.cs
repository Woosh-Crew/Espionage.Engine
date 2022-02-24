﻿using UnityEngine;

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

			// Start Gravity
			Velocity -= new Vector3( 0, gravity / 2, 0 ) * Time.deltaTime;

			if ( Controller.isGrounded )
			{
				Velocity = Velocity.WithY( 0 );
			}

			// Smooth WishSpeed, so it isn't jarring
			WishSpeed = Mathf.Lerp( WishSpeed, GrabWishSpeed( client ), 6 * Time.deltaTime );

			var wishDir = rot * Vector3.forward * input.Forward + rot * Vector3.right * input.Horizontal;
			wishDir = wishDir.normalized * WishSpeed * Time.deltaTime;

			Velocity = wishDir;

			// Finish Gravity
			Velocity -= new Vector3( 0, gravity / 2, 0 ) * Time.deltaTime;

			if ( Controller.isGrounded )
			{
				Velocity = Velocity.WithY( 0 );
			}

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

		// Move

		protected virtual void GroundedMove() { }

		// Fields

		[SerializeField]
		private float walkSpeed = 15;

		[SerializeField]
		private float sprintSpeed = 15;

		[SerializeField]
		private float gravity = 20;

		[SerializeField]
		private float friction = 10;

		[SerializeField]
		private float stopSpeed = 100;
	}
}
