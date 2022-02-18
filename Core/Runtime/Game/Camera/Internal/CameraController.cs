using System;
using UnityEngine;

namespace Espionage.Engine
{
	[RequireComponent( typeof( Camera ), typeof( AudioListener ), typeof( FlareLayer ) )]
	internal class CameraController : MonoBehaviour
	{
		private Camera _cam;
		private Camera _viewmodelCam;

		private void Awake()
		{
			gameObject.tag = "MainCamera";
			_cam = GetComponent<Camera>();
			_cam.depth = 2;

			// Viewmodel Camera
			var viewmodelObj = new GameObject( "Viewmodel Camera" );
			viewmodelObj.transform.parent = transform;
			_viewmodelCam = viewmodelObj.AddComponent<Camera>();

			_viewmodelCam.clearFlags = CameraClearFlags.Depth;
			_viewmodelCam.cullingMask = LayerMask.GetMask( "Viewmodel", "TransparentFX" );
			_viewmodelCam.depth = 4;

			_viewmodelCam.nearClipPlane = 0.1f;
			_viewmodelCam.farClipPlane = 10;
		}

		private Transform _lastViewer;

		public void Finalise( ITripod.Setup camSetup )
		{
			var trans = transform;
			trans.localPosition = camSetup.Position;
			trans.localRotation = camSetup.Rotation;

			_cam.fieldOfView = camSetup.Damping > 0 ? Mathf.Lerp( _cam.fieldOfView, camSetup.FieldOfView, camSetup.Damping * Time.deltaTime ) : camSetup.FieldOfView;
			_viewmodelCam.fieldOfView = _cam.fieldOfView;

			if ( _lastViewer != camSetup.Viewer )
			{
				if ( _lastViewer != null )
				{
					_lastViewer.gameObject.SetActive( true );
				}

				_lastViewer = camSetup.Viewer;

				if ( _lastViewer != null )
				{
					_lastViewer.gameObject.SetActive( false );
				}
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere( transform.position, 0.8f );

			// This is hacky.. But who cares
			Callback.Run( "debug.gizmos" );
		}
	}
}
