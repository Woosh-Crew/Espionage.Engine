using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public sealed class SimpleSway : Behaviour, Viewmodel.IEffect
	{
		private Vector2 _lastMouseDelta;

		public void PostCameraSetup( ref ITripod.Setup setup )
		{
			var mouse = new Vector2(
				Input.GetAxis( "Mouse X" ),
				Input.GetAxis( "Mouse Y" )
			);

			_lastMouseDelta = Vector2.Lerp( _lastMouseDelta, mouse, smoothing * Time.deltaTime );

			var trans = transform;
			var rotationX = Quaternion.AngleAxis( _lastMouseDelta.y * scale, Vector3.left );
			var rotationY = Quaternion.AngleAxis( _lastMouseDelta.x * scale, Vector3.up );

			trans.rotation *= rotationX * rotationY * Quaternion.AngleAxis( _lastMouseDelta.x * tilting, Vector3.forward );

			var localRotation = trans.localRotation;
			trans.position += localRotation * Vector3.down * _lastMouseDelta.y * (scale / 100) + localRotation * Vector3.left * _lastMouseDelta.x * (scale / 100);
		}

		// Fields

		[SerializeField]
		private float tilting = 1;

		[SerializeField]
		private float scale = 10;

		[SerializeField]
		private float smoothing = 10;
	}
}
