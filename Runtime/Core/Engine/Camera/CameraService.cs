using Espionage.Engine.Internal;
using UnityEngine;

namespace Espionage.Engine.Services
{
	[Order( -5 ), Title( "Cameras" )]
	public class CameraService : Service
	{
		public Camera Camera => _camController.Camera;

		public override void OnReady()
		{
			// Main Camera
			_camController = Entity.Create<CameraController>();
			Engine.Scene.Grab( entity: _camController );

			// Tell everyone we got cameras

			Engine.Game.OnCameraCreated( _camController.Camera );
			Callback.Run( "camera.created", _camController.Camera );
		}

		// Frame

		private CameraController _camController;

		private Tripod.Setup _lastSetup = new()
		{
			FieldOfView = 68,
			Rotation = Quaternion.identity,
			Position = Vector3.zero,
			Viewmodel = new()
			{
				FieldOfView = 68,
				Clipping = new( 0.14f, 10 ),
				Offset = Vector3.zero,
				Angles = Quaternion.identity
			}
		};

		public override void OnPostUpdate()
		{
			// Default FOV
			_lastSetup.FieldOfView = 68;
			_lastSetup.Clipping = new( 0.1f, 700 );
			_lastSetup.Camera = _camController.Camera;

			// Viewmodels get Reset every frame.
			_lastSetup.Viewmodel.Offset = Vector3.zero;
			_lastSetup.Viewmodel.Angles = Quaternion.identity;

			// Build the camSetup, from game.
			_lastSetup = Engine.Game.BuildTripod( _lastSetup );
			_camController.Finalise( _lastSetup );
		}
	}
}
