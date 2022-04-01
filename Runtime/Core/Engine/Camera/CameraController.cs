using System;
using Espionage.Engine.Resources;
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

		private Entity _lastViewer;

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

			if ( _lastViewer != null && _lastViewer.Components.TryGet<Visuals>( out var oldVisuals ) && oldVisuals.Renderers != null )
			{
				foreach ( var meshRenderer in oldVisuals.Renderers )
				{
					meshRenderer.shadowCastingMode = ShadowCastingMode.On;
				}
			}

			_lastViewer = camSetup.Viewer;

			if ( _lastViewer != null && _lastViewer.Components.TryGet<Visuals>( out var newVisuals ) && newVisuals.Renderers != null )
			{
				foreach ( var meshRenderer in newVisuals.Renderers )
				{
					meshRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
				}
			}

			Viewmodel.Show( _lastViewer != null );
		}
	}
}
