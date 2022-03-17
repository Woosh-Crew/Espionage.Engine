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
			Vector2 mouse = new() { x = Input.GetAxis( "Mouse X" ), y = Input.GetAxis( "Mouse Y" ) };
			mouse *= Options.MouseSensitivity;

			_setup.MouseDelta = mouse;

			_setup.MouseWheel = Input.GetAxisRaw( "Mouse ScrollWheel" );

			_setup.Forward = Input.GetAxisRaw( "Vertical" );
			_setup.Horizontal = Input.GetAxisRaw( "Horizontal" );

			_setup = Engine.Game.BuildControls( _setup );
		}
	}
}
