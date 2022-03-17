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
			_controller = Library.Database.Create<CameraController>();
			Engine.Scene.Grab( _controller );
			_controller.name = "[ Viewport ] Main Camera";

			Engine.Game.OnCameraCreated( _controller.Camera );
			Callback.Run( "camera.created", _controller.Camera );
		}

		// Frame

		private CameraController _controller;

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
			_lastSetup = Engine.Game.BuildTripod( _lastSetup );

			// Finalise
			_controller.Finalise( _lastSetup );
		}
	}
}
