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
		private Vector3 _targetRot;

		void ITripod.Build( ref Tripod.Setup camSetup )
		{
			// Rotation
			camSetup.Rotation = Quaternion.Euler( _targetRot );

			// Movement
			var vel = camSetup.Rotation * _direction;
			vel = vel.normalized * 10;

			if ( Input.GetMouseButton( 1 ) )
			{
				if ( Input.GetKey( KeyCode.LeftShift ) )
				{
					vel *= 2;
				}

				if ( Input.GetKey( KeyCode.E ) )
				{
					vel += Vector3.up * 5;
				}

				if ( Input.GetKey( KeyCode.Q ) )
				{
					vel += Vector3.down * 5;
				}
			}

			_targetPos += vel * Time.deltaTime;

			camSetup.Position = camSetup.Position.LerpTo( _targetPos, 12 * Time.deltaTime );
		}

		public void Activated( ref Tripod.Setup camSetup )
		{
			_targetPos = camSetup.Position;
			_targetRot = camSetup.Rotation.eulerAngles.WithZ( 0 );
		}

		public void Deactivated() { }

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

				if ( setup.Mouse.Delta.magnitude >= 0.045f )
				{
					setup.Cursor.Visible = false;
					setup.Cursor.Locked = true;

					// We don't use ViewAngles here, as they are not our eyes.
					_targetRot += new Vector3( -setup.Mouse.Delta.y, setup.Mouse.Delta.x, 0 );
					_targetRot.x = Mathf.Clamp( _targetRot.x, -88, 88 );
				}
			}
			finally
			{
				setup.Clear();
			}
		}
	}
}
