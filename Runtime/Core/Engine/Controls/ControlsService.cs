using UnityEngine;

namespace Espionage.Engine.Services
{
	[Order( -10 )]
	internal class ControlsService : Service
	{
		private IControls.Setup _setup = new();

		[Property( "controls.ms_sensitivity" ), Title( "Mouse Sensitivity" ), ConVar, PrefVar]
		private static float MouseSensitivity { get; set; } = 2;

		public override void OnUpdate()
		{
			if ( Engine.Game == null || !Application.isPlaying )
			{
				return;
			}

			// Setup ViewAngles,
			_setup.MouseDelta = new()
			{
				x = Input.GetAxis( "Mouse X" ) * MouseSensitivity,
				y = Input.GetAxis( "Mouse Y" ) * MouseSensitivity
			};

			_setup.MouseWheel = Input.GetAxisRaw( "Mouse ScrollWheel" );

			_setup.Forward = Input.GetAxisRaw( "Vertical" );
			_setup.Horizontal = Input.GetAxisRaw( "Horizontal" );

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			_setup = Engine.Game.BuildControls( _setup );

			// This is really fucking stupid..
			// But is temp until we get networking
			// Going after netscape cyber mind
			Local.Client.Input = _setup;
		}
	}
}
