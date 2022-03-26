using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public sealed class SimpleSway : Viewmodel.Modifier
	{
		private Vector2 _lastMouseDelta;

		public override void PostCameraSetup( ref Tripod.Setup setup )
		{
			var mouse = Local.Client.Input.Mouse.Delta;

			_lastMouseDelta = Vector2.Lerp( _lastMouseDelta, mouse / 500 / Time.deltaTime, smoothing * Time.deltaTime );

			var rotationX = Quaternion.AngleAxis( _lastMouseDelta.y * scale.y, Vector3.left );
			var rotationY = Quaternion.AngleAxis( _lastMouseDelta.x * scale.x, Vector3.up );

			Rotation *= rotationX * rotationY * Quaternion.AngleAxis( _lastMouseDelta.x * tilting, Vector3.forward );
			Position += Rotation * Vector3.down * _lastMouseDelta.y * (scale.y / 100) + Rotation * Vector3.left * _lastMouseDelta.x * (scale.x / 100);
		}

		// Fields

		[SerializeField]
		private float tilting = 1;

		[SerializeField]
		private Vector2 scale = new( 10, 10 );

		[SerializeField]
		private float smoothing = 10;
	}
}
