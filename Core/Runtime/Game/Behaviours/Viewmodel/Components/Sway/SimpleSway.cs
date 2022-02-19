using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public sealed class SimpleSway : Behaviour, Viewmodel.IEffect
	{
		private Vector2 _lastMouseDelta;
		private Vector3 _lastSwayPos;

		public void PostCameraSetup( ref ITripod.Setup setup )
		{
			var mouse = new Vector2(
				Input.GetAxisRaw( "Mouse X" ),
				Input.GetAxisRaw( "Mouse Y" )
			);

			_lastMouseDelta = Vector2.Lerp( _lastMouseDelta, mouse, 10 * Time.deltaTime );

			var trans = transform;

			// calculate target rotation
			var rotationX = Quaternion.AngleAxis( _lastMouseDelta.y * 10, Vector3.left );
			var rotationY = Quaternion.AngleAxis( _lastMouseDelta.x * 10, Vector3.up );

			trans.rotation *= rotationX * rotationY;

			_lastSwayPos = trans.localRotation * Vector3.down * _lastMouseDelta.y * 0.1f + trans.localRotation * Vector3.left * _lastMouseDelta.x * 0.1f;
			trans.position += _lastSwayPos;
		}

		// Fields
	}
}
