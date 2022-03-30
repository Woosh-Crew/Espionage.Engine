using System;
using UnityEngine;

namespace Espionage.Engine
{
	public class FirstPersonController : Pawn.Controller
	{
		public float WalkSpeed { get; set; } = 7;
		public float SprintSpeed { get; set; } = 12;

		// Logic

		private CharacterController _characterController;

		protected override void OnAttached( Pawn item )
		{
			if ( !item.TryGetComponent( out _characterController ) )
			{
				_characterController = item.gameObject.AddComponent<CharacterController>();

				_characterController.skinWidth = 0.004f;
				_characterController.radius = 0.4f;
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
			var length = wishDirection.magnitude;

			wishDirection = wishDirection.normalized;
			wishDirection *= Time.deltaTime;

			wishDirection *= length;

			Velocity = wishDirection;
			Velocity += new Vector3( 0, -9, 0 ) * Time.deltaTime;

			_characterController.Move( Velocity );
		}

		private float Speed()
		{
			if ( Input.GetKey( KeyCode.LeftShift ) )
			{
				return SprintSpeed;
			}

			return WalkSpeed;
		}
	}
}
