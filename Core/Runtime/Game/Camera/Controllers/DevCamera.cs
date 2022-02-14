using UnityEngine;

namespace Espionage.Engine.Cameras
{
	public class DevCamera : ICamera
	{
		private Vector3 _targetPos;
		private Vector2 _targetRot;

		public void Build( ref ICamera.Setup camSetup )
		{
			var input = Local.Client.Input;

			// FOV

			if ( Input.GetMouseButton( 1 ) )
			{
				camSetup.FieldOfView += -Input.GetAxisRaw( "Mouse Y" ) * 150 * Time.deltaTime;
				return;
			}

			_targetRot += input.ViewAngles * (camSetup.FieldOfView / 120);
			_targetRot.y = Mathf.Clamp( _targetRot.y, -88, 88 );

			camSetup.Rotation = Quaternion.AngleAxis( _targetRot.x, Vector3.up ) * Quaternion.AngleAxis( _targetRot.y, Vector3.left );

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

			// WTF? This makes no sense
			var rot = camSetup.Rotation.eulerAngles;
			_targetRot = new Vector2( rot.y, -rot.x );
		}

		public void Deactivated() { }
	}
}
