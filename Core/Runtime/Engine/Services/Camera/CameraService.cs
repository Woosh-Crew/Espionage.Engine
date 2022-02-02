using UnityEngine;

namespace Espionage.Engine.Services.Camera
{
	internal class CameraService : IService
	{
		// Ready

		public void OnReady()
		{
			if ( !Application.isPlaying )
			{
				return;
			}

			var obj = new GameObject( "Main Camera" );
			_camera = obj.AddComponent<CameraController>();

			Engine.AddToLayer( obj );
		}

		public void OnShutdown() { }

		// Frame

		private CameraController _camera;

		private Tripod.Setup _lastSetup = new()
		{
			Rotation = Quaternion.identity,
			FieldOfView = 74,
			Position = Vector3.zero
		};

		public void OnUpdate()
		{
			if ( Engine.Game == null )
			{
				return;
			}

			// Build the camSetup, from game.
			_lastSetup = Engine.Game.BuildCamera( _lastSetup );

			// Get Camera Component
			_camera.Finalise( _lastSetup );
		}
	}
}
