using UnityEngine;

namespace Espionage.Engine.Services
{
	internal class ControlsService : Service
	{
		private IControls.Setup _setup = new();


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
			_setup.MouseDelta = new()
			{
				x = Input.GetAxis( "Mouse X" ) * Options.MouseSensitivity,
				y = Input.GetAxis( "Mouse Y" ) * Options.MouseSensitivity
			};

			_setup.MouseWheel = Input.GetAxisRaw( "Mouse ScrollWheel" );

			_setup.Forward = Input.GetAxisRaw( "Vertical" );
			_setup.Horizontal = Input.GetAxisRaw( "Horizontal" );

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			_setup = Engine.Game.BuildControls( _setup );
		}
	}
}
