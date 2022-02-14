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

			// We should put this in the input processor...
			_rawRotation.x += Input.GetAxis( xAxis ) * 2;
			_rawRotation.y += Input.GetAxis( yAxis ) * 2;
			_rawRotation.y = Mathf.Clamp( _rawRotation.y, -88, 88 );

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			Rotation = Quaternion.AngleAxis( _rawRotation.x, Vector3.up ) * Quaternion.AngleAxis( _rawRotation.y, Vector3.left );

			Forward = Input.GetAxisRaw( "Vertical" );
			Horizontal = -Input.GetAxisRaw( "Horizontal" );
		}

		public void OnShutdown() { }
		public void Dispose() { }

		//
		// Input Processor
		//

		private Vector2 _rawRotation = Vector2.zero;

		public Quaternion Rotation { get; set; }
		public float Forward { get; set; }
		public float Horizontal { get; set; }
	}
}
