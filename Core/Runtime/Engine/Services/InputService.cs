using UnityEngine;

namespace Espionage.Engine.Services
{
	internal class InputService : IService, IInputProcessor
	{
		public Library ClassInfo { get; }

		public InputService()
		{
			ClassInfo = Library.Database[GetType()];
		}

		//
		// Service
		//

		public void OnReady()
		{
			// This is really fucking stupid..
			// But is temp until we get networking
			// Going after netscape cyber mind
			Local.Client.Input = this;
		}

		public void OnUpdate()
		{
			const string xAxis = "Mouse X";
			const string yAxis = "Mouse Y";

			var newAngles = new Vector2();

			// We should put this in the input processor...
			newAngles.x = Input.GetAxis( xAxis ) * 2;
			newAngles.y = Input.GetAxis( yAxis ) * 2;
			// newAngles.y = 

			ViewAngles = newAngles;

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;


			Forward = Input.GetAxisRaw( "Vertical" );
			Horizontal = -Input.GetAxisRaw( "Horizontal" );
		}

		public void OnShutdown() { }
		public void Dispose() { }

		//
		// Input Processor
		//

		public Vector2 ViewAngles { get; set; }
		public float Forward { get; set; }
		public float Horizontal { get; set; }
	}
}
