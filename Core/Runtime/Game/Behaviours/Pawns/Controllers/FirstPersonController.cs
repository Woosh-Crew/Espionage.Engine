using UnityEngine;

namespace Espionage.Engine
{
	public class FirstPersonController : PawnController
	{
		[SerializeField]
		private float speed = 15;

		protected override void OnAwake()
		{
			Controller = GetComponent<CharacterController>();
		}

		public CharacterController Controller { get; private set; }

		public override void Simulate( Client client )
		{
			base.Simulate( client );

			var input = client.Input;

			var localRotation = transform.localRotation;
			var wishDir = localRotation * Vector3.forward * input.Forward + localRotation * Vector3.right * input.Horizontal;

			if ( !Controller.isGrounded )
			{
				wishDir += Vector3.down;
			}

			wishDir = wishDir.normalized * speed * Time.deltaTime;

			Controller.Move( wishDir );
		}
	}
}
