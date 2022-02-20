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
			var wishDir = rot * Vector3.forward * input.Forward + rot * Vector3.right * input.Horizontal;

			if ( !Controller.isGrounded )
			{
				wishDir += Vector3.down;
			}

			// Smooth WishSpeed, so it isn't jarring
			WishSpeed = Mathf.Lerp( WishSpeed, GrabWishSpeed( client ), 6 * Time.deltaTime );

			wishDir = wishDir.normalized * WishSpeed * Time.deltaTime;

			Controller.Move( wishDir );
		}

		// Wish Speed

		public float WishSpeed { get; protected set; }

		protected virtual float GrabWishSpeed( Client cl )
		{
			if ( cl.Input.Forward != 0 || cl.Input.Horizontal != 0 && Input.GetKey( KeyCode.LeftShift ) )
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
