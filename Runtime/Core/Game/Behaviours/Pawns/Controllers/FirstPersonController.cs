using UnityEngine;

namespace Espionage.Engine
{
	public class FirstPersonController : Component<Pawn>, Pawn.IController
	{
		private CharacterController _characterController;

		protected override void OnAwake()
		{
			if ( !TryGetComponent( out _characterController ) )
			{
				_characterController = gameObject.AddComponent<CharacterController>();
				_characterController.height = 1.8f;
				_characterController.center = _characterController.center.WithY( 1.8f / 2 );
			}
		}

		public void Simulate( Client client, Pawn pawn )
		{
			// player movement - forward, backward, left, right
			Vector2 input = new( client.Input.Horizontal, client.Input.Forward );
			input = input.normalized;
			input *= movementSpeed;

			Entity.Velocity = transform.rotation * (Vector3.right * input.x + Vector3.forward * input.y) * Time.deltaTime;

			Entity.Velocity = Entity.Velocity.WithY( Mathf.Sqrt( Entity.Velocity.y ) + -gravity * 2 * Time.deltaTime );

			// Gravity
			if ( _characterController.isGrounded )
			{
				Entity.Velocity = Entity.Velocity.WithY( 0 );
			}

			_characterController.Move( Entity.Velocity );
		}

		// Fields

		[SerializeField]
		private float movementSpeed = 1;

		[SerializeField]
		private float gravity = 9.8f;

		[SerializeField]
		private float jumpHeight = 1f;
	}
}
