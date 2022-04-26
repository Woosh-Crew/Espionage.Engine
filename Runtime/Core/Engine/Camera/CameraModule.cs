using Espionage.Engine.Resources;
using UnityEngine;
using UnityEngine.Rendering;

namespace Espionage.Engine
{
	[Order( -5 ), Title( "Cameras" )]
	public class CameraModule : Module
	{
		private Controller _controller;
		public Camera Camera => _controller.Camera;

		protected override void OnReady()
		{
			// Main Camera
			_controller = Entity.Create<Controller>();
			Engine.Scene.Grab( entity : _controller );

			// Tell everyone we got cameras
			Engine.Project.OnCameraCreated( _controller.Camera );
			Callback.Run( "camera.created", _controller.Camera );
		}

		// Frame

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

		protected override void OnUpdate()
		{
			// Default FOV
			_lastSetup.FieldOfView = 68;
			_lastSetup.Clipping = new( 0.1f, 700 );
			_lastSetup.Camera = _controller.Camera;

			// Viewmodels get Reset every frame.
			_lastSetup.Viewmodel.Offset = Vector3.zero;
			_lastSetup.Viewmodel.Angles = Quaternion.identity;

			// Build the camSetup, from game.
			_lastSetup = Engine.Project.BuildTripod( _lastSetup );
			_controller.Finalise( _lastSetup );
		}

		// Entities

		[Library( "camera.controller" ), Group( "Engine" ), Singleton]
		private class Controller : Entity
		{
			internal Camera Camera { get; private set; }

			public override void Spawn()
			{
				GameObject.tag = "MainCamera";
				Camera = GameObject.AddComponent<Camera>();
				GameObject.AddComponent<AudioListener>();
				Camera.depth = 5;
			}

			private Entity _lastViewer;

			internal void Finalise( in Tripod.Setup camSetup )
			{
				var trans = Transform;
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
}
