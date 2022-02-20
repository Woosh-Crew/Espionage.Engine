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

		// Fields

		[SerializeField]
		private float walkSpeed = 15;

		[SerializeField]
		private float sprintSpeed = 15;
	}
}
