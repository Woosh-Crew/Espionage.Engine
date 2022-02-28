using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public sealed class SimpleSway : Behaviour, Viewmodel.IEffect
	{
		private Vector2 _lastMouseDelta;

		public void PostCameraSetup( ref ITripod.Setup setup )
		{
			var mouse = new Vector2(
				Input.GetAxisRaw( "Mouse X" ),
				Input.GetAxisRaw( "Mouse Y" )
			);

			_lastMouseDelta = Vector2.Lerp( _lastMouseDelta, mouse, 10 * Time.deltaTime );

			var trans = transform;
			var rotationX = Quaternion.AngleAxis( _lastMouseDelta.y * scale, Vector3.left );
			var rotationY = Quaternion.AngleAxis( _lastMouseDelta.x * scale, Vector3.up );

			trans.rotation *= rotationX * rotationY;

			var localRotation = trans.localRotation;
			trans.position += localRotation * Vector3.down * _lastMouseDelta.y * (scale / 10) + localRotation * Vector3.left * _lastMouseDelta.x * (scale / 10);
		}

		// Fields

		[SerializeField]
		private float scale = 10;
	}
}
