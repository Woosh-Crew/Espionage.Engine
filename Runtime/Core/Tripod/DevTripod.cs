using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tripods
{
	[Library( "tripod.dev" ), Group( "Tripods" ), Title( "Dev Cam" )]
	public class DevTripod : ITripod, IControls, ILibrary
	{
		public Library ClassInfo { get; }

		public DevTripod()
		{
			ClassInfo = Library.Register( this );
		}

		// Tripod

		private Vector2 _direction;
		private Vector3 _targetPos;
		private Vector3 _targetRot;

		private bool _interpolate;

		private bool _changeFov;
		private float _targetFov;

		private Entity _frontEntity;

		private float _speedMulti = 1;

		void ITripod.Build( ref Tripod.Setup camSetup )
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

			// Dev stuff in front of us.
			if ( Physics.Raycast( camSetup.Position, camSetup.Rotation * Vector3.forward, out var hit, 20 ) )
			{
				_frontEntity = null;

				if ( hit.collider != null && hit.collider.TryGetComponent<Entity>( out var entity ) )
				{
					_frontEntity = entity;
				}
			}
		}

		public void Activated( ref Tripod.Setup camSetup )
		{
			_targetPos = camSetup.Position;
			_targetRot = camSetup.Rotation.eulerAngles.WithZ( 0 );
			_targetFov = camSetup.FieldOfView;
		}

		public void Deactivated() { }

		// Input

		void IControls.Build( Controls.Setup setup )
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
				_targetRot += new Vector3( -setup.Mouse.Delta.y, setup.Mouse.Delta.x, 0 );
				_targetRot.x = Mathf.Clamp( _targetRot.x, -88, 88 );

				if ( Input.GetMouseButtonDown( 2 ) )
				{
					_interpolate = !_interpolate;
				}

				_speedMulti += setup.Mouse.Wheel * 2;
				_speedMulti = Mathf.Clamp( _speedMulti, 0.1f, 10 );
			}
			finally
			{
				setup.Clear();
			}
		}

		// UI

		[Function, Callback( "imgui.layout" )]
		private void Layout()
		{
			// Get Padding
			const float Padding = 10.0f;
			var viewport = ImGui.GetMainViewport();

			var workPos = viewport.WorkPos; // Use work area to avoid menu-bar/task-bar, if any!
			Vector2 windowPos = new() { x = workPos.x + Padding, y = workPos.y + Padding };

			ImGui.SetNextWindowPos( windowPos, ImGuiCond.Always, Vector2.zero );

			// Create Window
			var flags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoNav | ImGuiWindowFlags.NoMove;

			ImGui.SetNextWindowBgAlpha( 0.35f );
			if ( ImGui.Begin( "Dev Tripod", flags ) )
			{
				ImGui.Text( "[ Developer Tripod ]" );
				ImGui.Separator();
				ImGui.Text( $"Fov: {(int)_targetFov}\nSpeed: {_speedMulti}" );

				if ( _frontEntity != null )
				{
					ImGui.Separator();
					ImGui.Text( $"[{_frontEntity.ClassInfo.Group}] {_frontEntity.ClassInfo.Title}\n[{_frontEntity.ClassInfo.Name}]" );

					if ( _frontEntity is Pawn pawn )
					{
						ImGui.Separator();
						ImGui.Text( "Possess Pawn with [F]" );
						if ( Input.GetKeyDown( KeyCode.F ) )
						{
							Dev.Terminal.Invoke( "dev.tripod" );
							Local.Client.Pawn = pawn;
						}
					}
				}
			}

			ImGui.End();
		}
	}
}
