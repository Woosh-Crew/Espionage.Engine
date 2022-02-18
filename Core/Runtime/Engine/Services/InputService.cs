using UnityEngine;

namespace Espionage.Engine.Services
{
	internal class InputService : IService
	{
		public Library ClassInfo { get; }

		public InputService()
		{
			ClassInfo = Library.Database[GetType()];
		}

		//
		// Service
		//

		private IControls.Setup _setup;

		public void OnReady()
		{
			// This is really fucking stupid..
			// But is temp until we get networking
			// Going after netscape cyber mind
			Local.Client.Input = _setup;
		}

		public void OnUpdate()
		{
			if ( Engine.Game == null || !Application.isPlaying )
			{
				return;
			}

			// Setup ViewAngles
			const string xAxis = "Mouse X";
			const string yAxis = "Mouse Y";

			var newAngles = new Vector2
			{
				x = Input.GetAxis( xAxis ) * 2,
				y = Input.GetAxis( yAxis ) * 2
			};

			_setup.ViewAngles = newAngles;

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			_setup.Forward = Input.GetAxisRaw( "Vertical" );
			_setup.Horizontal = Input.GetAxisRaw( "Horizontal" );

			_setup = Engine.Game.BuildControls( _setup );
		}

		public void OnShutdown() { }
		public void Dispose() { }
	}
}
