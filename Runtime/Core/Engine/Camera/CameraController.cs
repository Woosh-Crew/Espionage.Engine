using UnityEngine;
using UnityEngine.Rendering;

namespace Espionage.Engine.Internal
{
	[Group( "Engine" ), Singleton, RequireComponent( typeof( Camera ), typeof( AudioListener ) )]
	public class CameraController : Entity
	{
		internal Camera Camera { get; private set; }

		protected override void OnAwake()
		{
			gameObject.tag = "MainCamera";
			Camera = GetComponent<Camera>();
			Camera.depth = 5;
		}

		private Transform _lastViewer;

		internal void Finalise( in Tripod.Setup camSetup )
		{
			var trans = transform;
			trans.localPosition = camSetup.Position;
			trans.localRotation = camSetup.Rotation;

			Camera.fieldOfView = camSetup.Damping > 0 ? Mathf.Lerp( Camera.fieldOfView, camSetup.FieldOfView, camSetup.Damping * Time.deltaTime ) : camSetup.FieldOfView;

			// Clipping Planes

			Camera.farClipPlane = camSetup.Clipping.y;
			Camera.nearClipPlane = camSetup.Clipping.x;

			// Viewer

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
	}
}
