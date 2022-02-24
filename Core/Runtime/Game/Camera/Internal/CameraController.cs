using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Espionage.Engine
{
	[RequireComponent( typeof( Camera ), typeof( AudioListener ), typeof( FlareLayer ) )]
	internal class CameraController : MonoBehaviour
	{
		private Camera _cam;

		private void Awake()
		{
			gameObject.tag = "MainCamera";
			_cam = GetComponent<Camera>();
			_cam.depth = 2;

			Engine.Game.OnCameraCreated( _cam );
		}

		private Transform _lastViewer;

		public void Finalise( ITripod.Setup camSetup )
		{
			var trans = transform;
			trans.localPosition = camSetup.Position;
			trans.localRotation = camSetup.Rotation;

			_cam.fieldOfView = camSetup.Damping > 0 ? Mathf.Lerp( _cam.fieldOfView, camSetup.FieldOfView, camSetup.Damping * Time.deltaTime ) : camSetup.FieldOfView;

			if ( _lastViewer != camSetup.Viewer )
			{
				Debugging.Log.Info( "New Viewer" );

				if ( _lastViewer != null )
				{
					foreach ( var meshRenderer in _lastViewer.GetComponentsInChildren<Renderer>() )
					{
						meshRenderer.shadowCastingMode = ShadowCastingMode.On;
					}
				}

				_lastViewer = camSetup.Viewer;

				if ( _lastViewer != null )
				{
					foreach ( var meshRenderer in _lastViewer.GetComponentsInChildren<Renderer>() )
					{
						meshRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
					}
				}

				Viewmodel.Show( _lastViewer != null );
			}
		}

		private void OnDrawGizmos()
		{
			var position = transform.position;
			var rotation = transform.rotation;

			Gizmos.DrawWireSphere( position, 0.2f );

			Gizmos.color = Color.red;
			Gizmos.DrawLine( position, rotation * Vector3.forward + position );
			Gizmos.DrawLine( position, rotation * Vector3.forward + position + rotation * Vector3.left );
			Gizmos.DrawLine( position, rotation * Vector3.forward + position + rotation * Vector3.right );
			Gizmos.DrawLine( position, rotation * Vector3.forward + position + rotation * Vector3.up );
			Gizmos.DrawLine( position, rotation * Vector3.forward + position + rotation * Vector3.down );
			Gizmos.color = Color.white;

			// This is hacky.. But who cares
			Callback.Run( "debug.gizmos" );
		}
	}
}
