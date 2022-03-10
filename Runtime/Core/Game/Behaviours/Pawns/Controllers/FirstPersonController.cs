using UnityEngine;

namespace Espionage.Engine
{
	public class FirstPersonController : Component<Pawn>, Pawn.IController
	{
		private CharacterController _characterController;
		private float _velocity = 0;

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

			_characterController.Move( transform.rotation * (Vector3.right * input.x + Vector3.forward * input.y) * Time.deltaTime );

			_velocity -= gravity * Time.deltaTime;

			// Gravity
			if ( _characterController.isGrounded )
			{
				_velocity = 0;
			}

			_characterController.Move( new( 0, _velocity, 0 ) );
		}

		// Fields

		[SerializeField]
		private float movementSpeed = 1;

		[SerializeField]
		private float gravity = 9.8f;
	}
}
