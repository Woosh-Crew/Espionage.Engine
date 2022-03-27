using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tripods
{
	[Library( "tripod.dev" ), Group( "Tripods" ), Title( "Dev Cam" )]
	public class DevTripod : ITripod, IControls
	{
		public Library ClassInfo { get; }

		public DevTripod()
		{
			ClassInfo = Library.Register( this );
		}

		// Tripod

		private Vector3 _direction;
		private Vector3 _targetPos;

		private float _targetFov;

		private Vector3 _viewAngles;
		private Quaternion _targetRot;

		private Transform _lastViewer;

		void ITripod.Build( ref Tripod.Setup camSetup )
		{
			// Field Of View
			camSetup.Damping = 10;
			camSetup.FieldOfView = _targetFov;

			// Viewer
			camSetup.Viewer = null;
			if ( _lastViewer != null )
			{
				camSetup.Viewer = _lastViewer;
			}

			// Rotation
			camSetup.Rotation = _targetRot;

			// Movement
			var vel = camSetup.Rotation * _direction;
			vel = vel.normalized * 10;

			if ( Input.GetMouseButton( 1 ) )
			{
				if ( Input.GetKey( KeyCode.E ) )
				{
					vel += Vector3.up * 5;
				}

				if ( Input.GetKey( KeyCode.Q ) )
				{
					vel += Vector3.down * 5;
				}

				if ( Input.GetKey( KeyCode.LeftShift ) )
				{
					vel *= 2;
				}
			}

			_targetPos += vel * Time.deltaTime;

			camSetup.Position = camSetup.Position.LerpTo( _targetPos, 12 * Time.deltaTime );
		}

		public void Activated( ref Tripod.Setup camSetup )
		{
			_targetPos = camSetup.Position;
			_targetRot = camSetup.Rotation;

			_viewAngles = camSetup.Rotation.eulerAngles;

			_savedLock = Controls.Cursor.Locked;
			_savedVis = Controls.Cursor.Visible;

			_targetFov = 68;

			_lastViewer = null;

			if ( camSetup.Viewer != null )
			{
				_lastViewer = camSetup.Viewer;
			}
		}

		public void Deactivated()
		{
			Controls.Cursor.Locked = _savedLock;
			Controls.Cursor.Visible = _savedVis;
		}

		// Cursor

		private bool _savedLock;
		private bool _savedVis;

		// Input

		void IControls.Build( Controls.Setup setup )
		{
			try
			{
				if ( !Input.GetMouseButton( 1 ) )
				{
					setup.Cursor.Locked = false;
					setup.Cursor.Visible = true;

					_direction = Vector2.zero;
					return;
				}

				_direction = new( setup.Horizontal, 0, setup.Forward );
				_direction *= 1.5f;

				_targetFov -= setup.Mouse.Wheel * 20;
				_targetFov = Mathf.Clamp( _targetFov, 1, 120 );

				if ( setup.Mouse.Delta.magnitude >= 0.045f )
				{
					setup.Cursor.Visible = false;
					setup.Cursor.Locked = true;

					// We don't use ViewAngles here, as they are not our eyes.
					_viewAngles += new Vector3( -setup.Mouse.Delta.y, setup.Mouse.Delta.x, 0 );
					_viewAngles.x = Mathf.Clamp( _viewAngles.x, -88, 88 );

					_targetRot = Quaternion.Euler( _viewAngles );
				}
			}
			finally
			{
				setup.Clear();
			}
		}
	}
}
