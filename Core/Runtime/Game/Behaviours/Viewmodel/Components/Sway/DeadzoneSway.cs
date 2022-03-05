using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public class DeadzoneSway : Behaviour, Viewmodel.IEffect
	{
		private Vector2 _savedDeadzoneAxis;
		private Quaternion _lastDeadzoneRotation;

		public void PostCameraSetup( ref ITripod.Setup setup )
		{
			DeadzoneAxis();

			if ( autoCenter )
			{
				_savedDeadzoneAxis.x = Easing.Linear( _savedDeadzoneAxis.x, 0, returnSpeed * Time.deltaTime );
				_savedDeadzoneAxis.y = Easing.Linear( _savedDeadzoneAxis.y, 0, returnSpeed * Time.deltaTime );
			}

			_lastDeadzoneRotation = Quaternion.Slerp( _lastDeadzoneRotation, Quaternion.Euler( _savedDeadzoneAxis.x, _savedDeadzoneAxis.y, 0 ), damping * Time.deltaTime );

			var trans = transform;
			var rotation = trans.rotation;

			rotation *= _lastDeadzoneRotation;
			trans.position += rotation * Vector3.up * _lastDeadzoneRotation.x + rotation * Vector3.right * _lastDeadzoneRotation.y;

			trans.rotation = rotation;
		}

		protected void DeadzoneAxis()
		{
			var mouse = new Vector2( Input.GetAxis( "Mouse X" ), Input.GetAxis( "Mouse Y" ) );

			_savedDeadzoneAxis.x += mouse.y * 20 * multiplier * Time.deltaTime;
			_savedDeadzoneAxis.x = Mathf.Clamp( _savedDeadzoneAxis.x, -deadzone.x, deadzone.x );

			_savedDeadzoneAxis.y += mouse.x * 20 * multiplier * Time.deltaTime;
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
