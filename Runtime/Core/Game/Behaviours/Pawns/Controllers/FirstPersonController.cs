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
			var input = new Vector2( Client.Input.Horizontal, Client.Input.Forward ).normalized;
			input *= movementSpeed;

			Velocity = transform.rotation * (Vector3.right * input.x + Vector3.forward * input.y) * Time.deltaTime;
			Velocity = Entity.Velocity.WithY( Velocity.y - gravity * 2 * Time.deltaTime );

			// Gravity
			if ( _characterController.isGrounded )
			{
				Velocity = Velocity.WithY( 0 );
			}

			_characterController.Move( Velocity );
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
