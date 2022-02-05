using UnityEngine;

namespace Espionage.Engine.Controllers
{
	public class DevCamera : ICamera
	{
		private Vector3 _targetPos;
		
		public void Build( ref ICamera.Setup camSetup )
		{
			var input = Local.Client.Input;
			camSetup.Rotation = input.Rotation;

			var vel = camSetup.Rotation * Vector3.forward * input.Forward + camSetup.Rotation * Vector3.left * input.Horizontal;

			if ( Input.GetKey( KeyCode.Space ) || Input.GetKey( KeyCode.E ) )
			{
				vel += Vector3.up * 1;
			}

			if ( Input.GetKey( KeyCode.LeftControl ) || Input.GetKey( KeyCode.Q ) )
			{
				vel += Vector3.down * 1;
			}

			vel = vel.normalized * 20;

			if ( Input.GetKey( KeyCode.LeftShift ) )
			{
				vel *= 5.0f;
			}

			if ( Input.GetKey( KeyCode.LeftAlt ) )
			{
				vel *= 0.2f;
			}

			_targetPos += vel * Time.deltaTime;

			camSetup.Position = Vector3.Lerp( camSetup.Position, _targetPos, 5 * Time.deltaTime );
		}

		public void Activated( ICamera.Setup camSetup )
		{
			_targetPos = camSetup.Position;
		}
		
		public void Deactivated() {  }
	}
}
