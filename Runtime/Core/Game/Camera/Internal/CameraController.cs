using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Espionage.Engine
{
	[Group( "Engine" ), Singleton, RequireComponent( typeof( Camera ), typeof( AudioListener ), typeof( FlareLayer ) )]
	internal class CameraController : Behaviour
	{
		private Camera _cam;

		protected override void OnAwake()
		{
			gameObject.tag = "MainCamera";
			_cam = GetComponent<Camera>();
			_cam.depth = 2;

			Engine.Game.OnCameraCreated( _cam );
		}

		public void Finalise( in ITripod.Setup camSetup )
		{
			var trans = transform;
			trans.localPosition = camSetup.Position;
			trans.localRotation = camSetup.Rotation;

			_cam.fieldOfView = camSetup.Damping > 0 ? Mathf.Lerp( _cam.fieldOfView, camSetup.FieldOfView, camSetup.Damping * Time.deltaTime ) : camSetup.FieldOfView;

			_cam.farClipPlane = camSetup.Clipping.y;
			_cam.nearClipPlane = camSetup.Clipping.x;

			HandleViewer( camSetup );
		}

		private Transform _lastViewer;

		private void HandleViewer( in ITripod.Setup camSetup )
		{
			if ( _lastViewer == camSetup.Viewer )
			{
				return;
			}

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

		private void OnGUI()
		{
			Callback.Run( "imgui.draw" );
		}
	}
}
