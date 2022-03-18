using UnityEngine;

namespace Espionage.Engine.Internal
{
	[Group( "Engine" ), Singleton]
	public class ViewmodelController : Entity
	{
		internal Camera Camera { get; private set; }

		protected override void OnAwake()
		{
			Camera = gameObject.AddComponent<Camera>();
			Camera.depth = 8;
			
			Camera.nearClipPlane = 0.05f;
			Camera.farClipPlane = 15;

			Camera.clearFlags = CameraClearFlags.Depth;
			Camera.cullingMask = LayerMask.GetMask( "Viewmodel" );
		}
		
		internal void Finalise( in Tripod.Setup camSetup )
		{
			var trans = transform;
			trans.localPosition = camSetup.Position;
			trans.localRotation = camSetup.Rotation;

			Camera.fieldOfView = camSetup.Damping > 0 ? Mathf.Lerp( Camera.fieldOfView, camSetup.FieldOfView, camSetup.Damping * Time.deltaTime ) : camSetup.FieldOfView;

			Viewmodel.Show( camSetup.Viewer != null );
		}
	}
}
