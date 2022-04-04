﻿using UnityEngine;

namespace Espionage.Engine.Services
{
	[Order( -2 )]
	internal class ControlsService : Service
	{
		private Controls.Setup _setup = new() { Cursor = new() { Locked = true, Visible = false } };

		public override void OnReady()
		{
			Local.Client.Input = _setup;
			_setup.Scheme = Engine.Game.SetupControls();
		}

		public override void OnUpdate()
		{
			// Setup ViewAngles,
			var mouse = new Vector2( Input.GetAxis( "Mouse X" ), Input.GetAxis( "Mouse Y" ) ) * Options.MouseSensitivity;

			_setup.Mouse = new() { Delta = mouse, Wheel = Input.GetAxisRaw( "Mouse ScrollWheel" ) };
			_setup.Forward = Input.GetAxisRaw( "Vertical" );
			_setup.Horizontal = Input.GetAxisRaw( "Horizontal" );

			// Sample Scheme
			foreach ( var binding in _setup.Scheme )
			{
				binding.Sample();
			}

			// Building
			_setup = Engine.Game.BuildControls( _setup );

			// Applying
			UnityEngine.Cursor.visible = _setup.Cursor.Visible;
			UnityEngine.Cursor.lockState = _setup.Cursor.Locked ? CursorLockMode.Locked : CursorLockMode.None;

			if ( !_setup.Cursor.Locked && _setup.Cursor.Confined )
			{
				UnityEngine.Cursor.lockState = CursorLockMode.Confined;
			}
		}
	}
}
