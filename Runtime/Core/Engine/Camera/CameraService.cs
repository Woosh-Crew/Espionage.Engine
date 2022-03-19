using Espionage.Engine.Tripods;
using Espionage.Engine.Internal;
using UnityEngine;

namespace Espionage.Engine.Services
{
	[Order( -5 )]
	internal class CameraService : Service
	{
		public override void OnReady()
		{
			// Main Camera
			_camController = Library.Database.Create<CameraController>();

			Engine.Scene.Grab( _camController );
			_camController.name = "[ Viewport ] Main Camera";

			// Tell everyone we got cameras

			Engine.Game.OnCameraCreated( _camController.Camera );
			Callback.Run( "camera.created", _camController.Camera );
		}

		// Frame

		private CameraController _camController;

		private Tripod.Setup _lastSetup = new()
		{
			Rotation = Quaternion.identity,
			FieldOfView = 60,
			Position = Vector3.zero,
			Viewmodel = new() { FieldOfView = 60, Clipping = new( 0.05f, 10 ) }
		};

		public override void OnPostUpdate()
		{
			// Default FOV
			_lastSetup.FieldOfView = 68;
			_lastSetup.Viewer = null;
			_lastSetup.Clipping = new( 0.1f, 700 );
			_lastSetup.Camera = _camController.Camera;

			// Build the camSetup, from game.
			_lastSetup = Engine.Game.BuildTripod( _lastSetup );

			// Finalise
			_camController.Finalise( _lastSetup );
		}
	}
}
