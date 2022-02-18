using UnityEngine;

namespace Espionage.Engine.Services
{
	[Order( -10 )]
	internal class ControlsService : IService
	{
		public Library ClassInfo { get; }

		public ControlsService()
		{
			ClassInfo = Library.Database[GetType()];
		}

		//
		// Service
		//

		private IControls.Setup _setup = new();

		public void OnReady() { }

		public void OnUpdate()
		{
			if ( Engine.Game == null || !Application.isPlaying )
			{
				return;
			}

			// Setup ViewAngles
			_setup.MouseDelta = new Vector2
			{
				x = Input.GetAxis( "Mouse X" ) * 2,
				y = Input.GetAxis( "Mouse Y" ) * 2
			};

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

		public void OnShutdown() { }
		public void Dispose() { }
	}
}
