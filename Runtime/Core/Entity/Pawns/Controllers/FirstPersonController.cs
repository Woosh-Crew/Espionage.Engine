using System;
using UnityEngine;

namespace Espionage.Engine
{
	public class FirstPersonController : Pawn.Controller
	{
		private CharacterController _characterController;

		protected override void OnAttached( Pawn item )
		{
			if ( !item.TryGetComponent( out _characterController ) )
			{
				_characterController = item.gameObject.AddComponent<CharacterController>();
				_characterController.height = 1.8f;
				_characterController.center = _characterController.center.WithY( 1.8f / 2 );
			}
		}

		private Vector3 _dampedWishDir;

		protected override void Simulate()
		{
			base.Simulate();

			// player movement - forward, backward, left, right
			var input = new Vector2( Client.Input.Horizontal, Client.Input.Forward );
			input = input.normalized;

			var wishDirection = Rotation * (Vector3.right * input.x + Vector3.forward * input.y);
			wishDirection *= Speed();
			wishDirection *= Time.deltaTime;

			_dampedWishDir = _dampedWishDir.LerpTo( wishDirection, 12 * Time.deltaTime );

			Velocity = _dampedWishDir;
			Velocity += new Vector3( 0, -9, 0 ) * Time.deltaTime;

			_characterController.Move( Velocity );
		}

		public float Speed()
		{
			if ( Input.GetKey( KeyCode.LeftShift ) )
			{
				return sprintSpeed;
			}

			return walkSpeed;
		}


		// Fields

		[SerializeField]
		private float walkSpeed = 7;

		[SerializeField]
		private float sprintSpeed = 12;
	}
}
