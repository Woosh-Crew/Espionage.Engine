using UnityEngine;

namespace Espionage.Engine.Services
{
	internal class ControlsService : Service
	{
		private Controls.Setup _setup = new() { Cursor = new() { Locked = true, Visible = false } };

		public override void OnReady()
		{
			Local.Client.Input = _setup;
		}

		public override void OnUpdate()
		{
			if ( Engine.Game == null || !Application.isPlaying )
			{
				return;
			}

			// Setup ViewAngles,
			Vector2 mouse = new() { x = Input.GetAxis( "Mouse X" ), y = Input.GetAxis( "Mouse Y" ) };
			mouse *= Options.MouseSensitivity;

			_setup.Mouse = new() { Delta = mouse, Wheel = Input.GetAxisRaw( "Mouse ScrollWheel" ) };

			_setup.Forward = Input.GetAxisRaw( "Vertical" );
			_setup.Horizontal = Input.GetAxisRaw( "Horizontal" );

			// Building
			_setup = Engine.Game.BuildControls( _setup );

			// Applying
			UnityEngine.Cursor.visible = _setup.Cursor.Visible;
			UnityEngine.Cursor.lockState = _setup.Cursor.Locked ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
}
