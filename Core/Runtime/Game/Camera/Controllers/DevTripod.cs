﻿using UnityEngine;

namespace Espionage.Engine.Cameras
{
	public class DevTripod : ITripod, IControls
	{
		private Vector2 _direction;
		private Vector3 _targetPos;
		private Vector3 _targetRot;

		private bool _interpolate;

		private bool _changeFov;
		private float _fovChangeDelta;

		private float _speedMulti = 1;

		void ITripod.Build( ref ITripod.Setup camSetup )
		{
			// FOV
			camSetup.Damping = 15;

			if ( _changeFov )
			{
				camSetup.FieldOfView += _fovChangeDelta * 150 * Time.deltaTime;
			}

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
		}

		public void Activated( ref ITripod.Setup camSetup )
		{
			_targetPos = camSetup.Position;

			var euler = camSetup.Rotation.eulerAngles;
			euler.z = 0;
			_targetRot = euler;
		}

		public void Deactivated() { }

		// Input

		void IControls.Build( ref IControls.Setup setup )
		{
			try
			{
				_changeFov = Input.GetMouseButton( 1 );

				_direction = new Vector2( setup.Forward, setup.Horizontal ) / (_changeFov ? 2 : 1);

				if ( _changeFov )
				{
					_fovChangeDelta = -Input.GetAxisRaw( "Mouse Y" );
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
