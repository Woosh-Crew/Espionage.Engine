using Espionage.Engine.Cameras;
using UnityEngine;

namespace Espionage.Engine.Services
{
	[Order( -5 )]
	internal class CameraService : IService
	{
		public Library ClassInfo { get; }

		public CameraService()
		{
			ClassInfo = Library.Database[GetType()];
		}

		// Ready

		public void OnReady()
		{
			if ( !Application.isPlaying )
			{
				return;
			}

			var obj = new GameObject( "Main Camera" );
			Engine.AddToLayer( obj );

			// Setup Camera
			_camera = obj.AddComponent<CameraController>();
		}

		public void OnShutdown() { }

		// Frame

		private CameraController _camera;

		private ITripod.Setup _lastSetup = new()
		{
			Rotation = Quaternion.identity,
			FieldOfView = 74,
			Position = Vector3.zero
		};

		public void OnUpdate()
		{
			if ( Engine.Game == null || !Application.isPlaying )
			{
				return;
			}

			if ( Input.GetKeyDown( KeyCode.F1 ) )
			{
				Local.Client.Tripod = Local.Client.Tripod == null ? new DevTripod() : null;
			}

			// Build the camSetup, from game.
			_lastSetup = Engine.Game.BuildCamera( _lastSetup );

			// Finalise
			_camera.Finalise( _lastSetup );
		}

		public void Dispose() { }
	}
}
