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

			var wishDir = Pawn.EyeRot * Vector3.forward * input.Forward + Pawn.EyeRot * Vector3.left * input.Horizontal;

			if ( !Controller.isGrounded )
			{
				wishDir += Vector3.down;
			}

			wishDir.Normalize();
			wishDir *= Time.deltaTime;

			Controller.Move( wishDir );
		}
	}
}
