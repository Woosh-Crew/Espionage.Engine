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
			var input = client.Input;

			var wishDir = Pawn.EyeRot * Vector3.forward * input.Forward + Pawn.EyeRot * Vector3.left * input.Horizontal;

			Controller.Move( wishDir + Vector3.down * 20 );
		}
	}
}
