using UnityEngine;

namespace Espionage.Engine.Tripods
{
	[Library( "tripod.dev" ), Group( "Tripods" ), Title( "Dev Cam" )]
	public class DevTripod : ITripod, IControls
	{
		private Vector2 _direction;
		private Vector3 _targetPos;
		private Vector3 _targetRot;

		private bool _interpolate;

		private bool _changeFov;
		private float _targetFov;

		private float _speedMulti = 1;

		void ITripod.Build( ref ITripod.Setup camSetup )
		{
			// FOV
			camSetup.Damping = 15;
			camSetup.FieldOfView = _targetFov;

			// Rotation

			var finalRot = Quaternion.Euler( _targetRot );
			camSetup.Rotation = _interpolate ? Quaternion.Slerp( camSetup.Rotation, finalRot * Quaternion.AngleAxis( _direction.y * 3, Vector3.back ), 4 * Time.deltaTime ) : finalRot;

			// Movement

			var vel = camSetup.Rotation * Vector3.forward * _direction.x + camSetup.Rotation * Vector3.right * _direction.y;

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

			vel *= _speedMulti;

			_targetPos += vel * Time.deltaTime;

			camSetup.Position = _interpolate ? Vector3.Lerp( camSetup.Position, _targetPos, 2 * Time.deltaTime ) : Vector3.Lerp( camSetup.Position, _targetPos, 5 * Time.deltaTime );

			// Do shit in front of us
			if ( Physics.Raycast( camSetup.Position, camSetup.Rotation * Vector3.forward, out var hit, 20 ) && hit.collider.TryGetComponent<Entity>( out var entity ) )
			{
				Debugging.Overlay.Box( Vector2.zero, $"{entity.ClassInfo.Title}\n[{entity.ClassInfo.Group}]" );

				if ( entity is Pawn pawn && Input.GetKeyDown( KeyCode.F ) )
				{
					Local.Client.Pawn = pawn;
				}
			}
		}

		public void Activated( ref ITripod.Setup camSetup )
		{
			_targetPos = camSetup.Position;
			_targetRot = camSetup.Rotation.eulerAngles.WithZ( 0 );
			_targetFov = camSetup.FieldOfView;
		}

		public void Deactivated() { }

		// Input

		void IControls.Build( IControls.Setup setup )
		{
			try
			{
				_changeFov = Input.GetMouseButton( 1 );

				_direction = new Vector2( setup.Forward, setup.Horizontal ) / (_changeFov ? 2 : 1);

				if ( _changeFov )
				{
					_targetFov += -Input.GetAxisRaw( "Mouse Y" ) * 150 * Time.deltaTime;
					return;
				}

				// We don't use ViewAngles here, as they are not our eyes.
				_targetRot += new Vector3( -setup.MouseDelta.y, setup.MouseDelta.x, 0 );
				_targetRot.x = Mathf.Clamp( _targetRot.x, -88, 88 );

				if ( Input.GetMouseButtonDown( 2 ) )
				{
					_interpolate = !_interpolate;
				}

				_speedMulti += setup.MouseWheel * 2;
				_speedMulti = Mathf.Clamp( _speedMulti, 0.1f, 10 );
			}
			finally
			{
				setup.Clear();
			}
		}
	}
}
