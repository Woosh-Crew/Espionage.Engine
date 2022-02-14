using Espionage.Engine.Cameras;
using UnityEngine;

namespace Espionage.Engine.Services
{
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

			// Viewmodel
			var viewmodelObj = new GameObject( "Viewmodel Camera" );
			viewmodelObj.transform.parent = obj.transform;
			_viewmodelCam = viewmodelObj.AddComponent<Camera>();
			_viewmodelCam.clearFlags = CameraClearFlags.Depth;
			_viewmodelCam.cullingMask = LayerMask.GetMask( "Viewmodel", "TransparentFX" );
			_viewmodelCam.depth = 4;
			_viewmodelCam.farClipPlane = 10;
		}

		public void OnShutdown() { }

		// Frame

		private Camera _viewmodelCam;
		private CameraController _camera;

		private ICamera.Setup _lastSetup = new()
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
				Local.Client.Camera = Local.Client.Camera == null ? new DevCamera() : null;
			}

			// Build the camSetup, from game.
			_lastSetup = Engine.Game.BuildCamera( _lastSetup );

			// Finalise
			_viewmodelCam.fieldOfView = _lastSetup.FieldOfView;
			_camera.Finalise( _lastSetup );
		}
	}
}
