using Espionage.Engine.Tripods;
using Espionage.Engine.Internal;
using UnityEngine;

namespace Espionage.Engine.Services
{
	[Order( -5 )]
	public class CameraService : Service
	{
		public override void OnReady()
		{
			Controller = Library.Database.Create<CameraController>();
			Controller.gameObject.MoveTo( Engine.Scene );

			Engine.Game.OnCameraCreated( Controller.Camera );
			Callback.Run( "camera.created", Controller );
		}

		// Frame

		public CameraController Controller { get; private set; }

		private Tripod.Setup _lastSetup = new()
		{
			Rotation = Quaternion.identity,
			FieldOfView = 60,
			Position = Vector3.zero
		};

		public override void OnPostUpdate()
		{
			// Default FOV
			_lastSetup.FieldOfView = 68;
			_lastSetup.Viewer = null;
			_lastSetup.Clipping = new( 0.1f, 700 );


			// Build the camSetup, from game.
			_lastSetup = Engine.Game.BuildCamera( _lastSetup );

			// Finalise
			Controller.Finalise( _lastSetup );

			// Set the viewer to null, so its cleared every frame.
			_lastSetup.Viewer = null;
		}
	}
}
