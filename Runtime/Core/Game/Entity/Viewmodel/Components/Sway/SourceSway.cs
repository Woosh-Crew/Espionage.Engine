using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public class SourceSway : Viewmodel.Modifier
	{
		public float Smoothing { get; set; } = 10;
		public float Intensity { get; set; } = 0.2f;

		// Sway

		private Vector2 _lastMouseDelta;
		private Vector3 _lastPosition;

		public override void PostCameraSetup( ref Tripod.Setup setup )
		{
			_lastMouseDelta += Controls.Mouse.Delta * Time.deltaTime;
			_lastMouseDelta = _lastMouseDelta.LerpTo( Vector2.zero, Smoothing * Time.deltaTime );

			var vec = new Vector3( -_lastMouseDelta.x, -_lastMouseDelta.y, 0 );
			_lastPosition = _lastPosition.LerpTo( vec, 6 * Time.deltaTime );
			Position += _lastPosition * Intensity;
		}
	}
}
