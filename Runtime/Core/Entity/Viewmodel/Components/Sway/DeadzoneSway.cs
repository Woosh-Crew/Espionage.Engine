using Espionage.Engine.Tripods;
using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public class DeadzoneSway : Viewmodel.Modifier
	{
		[Property] private bool AutoCenter { get; set; } = true;
		[Property] private float ReturnSpeed { get; set; } = 2;
		[Property] private float Damping { get; set; } = 8;
		[Property] private float Multiplier { get; set; } = 1;
		[Property] private Vector2 Deadzone { get; set; } = new( 8, 8 );

		// Sway

		private Vector2 _savedDeadzoneAxis;
		private Quaternion _lastDeadzoneRotation;

		public override void PostCameraSetup( ref Tripod.Setup setup )
		{
			DeadzoneAxis();

			if ( AutoCenter )
			{
				_savedDeadzoneAxis.x = _savedDeadzoneAxis.x.LerpTo( 0, ReturnSpeed * Time.deltaTime );
				_savedDeadzoneAxis.y = _savedDeadzoneAxis.y.LerpTo( 0, ReturnSpeed * Time.deltaTime );
			}

			_lastDeadzoneRotation = Quaternion.Slerp( _lastDeadzoneRotation, Quaternion.Euler( _savedDeadzoneAxis.x, _savedDeadzoneAxis.y, 0 ), Damping * Time.deltaTime );

			Rotation *= _lastDeadzoneRotation;
			Position += Rotation * Vector3.up * (_lastDeadzoneRotation.x / 100) + Rotation * Vector3.right * (_lastDeadzoneRotation.y / 100);
		}

		private void DeadzoneAxis()
		{
			var mouse = Local.Client.Input.Mouse.Delta / Time.deltaTime;

			_savedDeadzoneAxis.x += -mouse.y * Multiplier * Time.deltaTime;
			_savedDeadzoneAxis.x = Mathf.Clamp( _savedDeadzoneAxis.x, -Deadzone.x, Deadzone.x );

			_savedDeadzoneAxis.y += mouse.x * Multiplier * Time.deltaTime;
			_savedDeadzoneAxis.y = Mathf.Clamp( _savedDeadzoneAxis.y, -Deadzone.y, Deadzone.y );
		}
	}
}
