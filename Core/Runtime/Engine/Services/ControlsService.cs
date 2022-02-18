using UnityEngine;

namespace Espionage.Engine.Services
{
	[Order( -5 )]
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

		public void OnReady() { }

		public void OnUpdate()
		{
			if ( Engine.Game == null || !Application.isPlaying )
			{
				return;
			}

			var setup = new IControls.Setup
			{
				// Setup ViewAngles
				MouseDelta = new Vector2
				{
					x = Input.GetAxis( "Mouse X" ) * 2,
					y = Input.GetAxis( "Mouse Y" ) * 2
				},

				// Setup Directional Input
				Forward = Input.GetAxisRaw( "Vertical" ),
				Horizontal = Input.GetAxisRaw( "Horizontal" )
			};

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			setup = Engine.Game.BuildControls( setup );

			// This is really fucking stupid..
			// But is temp until we get networking
			// Going after netscape cyber mind
			Local.Client.Input = setup;
		}

		public void OnShutdown() { }
		public void Dispose() { }
	}
}
