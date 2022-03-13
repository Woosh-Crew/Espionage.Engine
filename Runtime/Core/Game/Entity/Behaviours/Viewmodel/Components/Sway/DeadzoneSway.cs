using Espionage.Engine.Tripods;
using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public class DeadzoneSway : Viewmodel.Effect
	{
		private Vector2 _savedDeadzoneAxis;
		private Quaternion _lastDeadzoneRotation;

		public override void PostCameraSetup( ref Tripod.Setup setup )
		{
			DeadzoneAxis();

			if ( autoCenter )
			{
				_savedDeadzoneAxis.x = Easing.Linear( _savedDeadzoneAxis.x, 0, returnSpeed * Time.deltaTime );
				_savedDeadzoneAxis.y = Easing.Linear( _savedDeadzoneAxis.y, 0, returnSpeed * Time.deltaTime );
			}

			_lastDeadzoneRotation = Quaternion.Slerp( _lastDeadzoneRotation, Quaternion.Euler( _savedDeadzoneAxis.x, _savedDeadzoneAxis.y, 0 ), damping * Time.deltaTime );

			var trans = transform;
			trans.rotation *= _lastDeadzoneRotation;

			trans.position += trans.rotation * Vector3.up * (_lastDeadzoneRotation.x / 100) + trans.rotation * Vector3.right * (_lastDeadzoneRotation.y / 100);
		}

		private void DeadzoneAxis()
		{
			var mouse = Local.Client.Input.MouseDelta / Time.deltaTime;

			_savedDeadzoneAxis.x += -mouse.y * multiplier * Time.deltaTime;
			_savedDeadzoneAxis.x = Mathf.Clamp( _savedDeadzoneAxis.x, -deadzone.x, deadzone.x );

			_savedDeadzoneAxis.y += mouse.x * multiplier * Time.deltaTime;
			_savedDeadzoneAxis.y = Mathf.Clamp( _savedDeadzoneAxis.y, -deadzone.y, deadzone.y );
		}

		// Fields

		[SerializeField]
		private bool autoCenter = true;

		[SerializeField]
		private float returnSpeed = 2;

		[SerializeField]
		private float damping = 8;

		[SerializeField]
		private float multiplier = 1;

		[SerializeField]
		private Vector2 deadzone = new( 8, 8 );
	}
}
