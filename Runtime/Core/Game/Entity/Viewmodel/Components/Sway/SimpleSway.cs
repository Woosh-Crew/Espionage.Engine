using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public sealed class SimpleSway : Viewmodel.Modifier
	{
		private float Tilting { get; set; } = 1;
		private Vector2 Scale { get; set; } = new( 10, 10 );
		private float Smoothing { get; set; } = 10;

		// Sway

		private Vector2 _lastMouseDelta;

		public override void PostCameraSetup( ref Tripod.Setup setup )
		{
			var mouse = Local.Client.Input.Mouse.Delta;

			_lastMouseDelta = Vector2.Lerp( _lastMouseDelta, mouse / 500 / Time.deltaTime, Smoothing * Time.deltaTime );

			var rotationX = Quaternion.AngleAxis( _lastMouseDelta.y * Scale.y, Vector3.left );
			var rotationY = Quaternion.AngleAxis( _lastMouseDelta.x * Scale.x, Vector3.up );

			Rotation *= rotationX * rotationY * Quaternion.AngleAxis( _lastMouseDelta.x * Tilting, Vector3.forward );
			Position += Rotation.Down() * _lastMouseDelta.y * (Scale.y / 100) + Rotation.Left() * _lastMouseDelta.x * (Scale.x / 100);
		}
	}
}
