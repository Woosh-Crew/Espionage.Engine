using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public class DeadzoneSway : Behaviour, Viewmodel.IEffect
	{
		private Vector2 _savedDeadzoneAxis;
		private Quaternion _lastDeadzoneRotation;

		public void PostCameraSetup( ref ITripod.Setup setup )
		{
			var deadzoneBox = deadzone;

			DeadzoneAxis( deadzoneBox );

			if ( autoCenter)
			{
				_savedDeadzoneAxis.x = Easing.Linear( _savedDeadzoneAxis.x, 0, 2 * Time.deltaTime );
				_savedDeadzoneAxis.y = Easing.Linear( _savedDeadzoneAxis.y, 0, 2 * Time.deltaTime );
			}

			_lastDeadzoneRotation = Quaternion.Slerp( _lastDeadzoneRotation, Quaternion.Euler( _savedDeadzoneAxis.x, _savedDeadzoneAxis.y, 0 ), damping * Time.deltaTime );

			var trans = transform;
			trans.rotation *= _lastDeadzoneRotation;
			trans.position += trans.rotation * Vector3.up * _lastDeadzoneRotation.x + trans.rotation * Vector3.right * _lastDeadzoneRotation.y * 1;
		}

		protected void DeadzoneAxis( Vector2 deadZoneBox )
		{
			var mouse = new Vector2( Input.GetAxisRaw( "Mouse X" ), Input.GetAxisRaw( "Mouse Y" ) );
			mouse.Normalize();

			_savedDeadzoneAxis.x += mouse.y * 20 * multiplier * Time.deltaTime;
			_savedDeadzoneAxis.x = Mathf.Clamp( _savedDeadzoneAxis.x, -deadZoneBox.x, deadZoneBox.x );

			_savedDeadzoneAxis.y += -mouse.x * 20 * multiplier * Time.deltaTime;
			_savedDeadzoneAxis.y = Mathf.Clamp( _savedDeadzoneAxis.y, -deadZoneBox.y, deadZoneBox.y );

		}

		// Fields


		[SerializeField]
		private bool autoCenter = true;

		[SerializeField]
		private float damping = 8;

		[SerializeField]
		private float multiplier = 1;

		[SerializeField]
		private Vector2 deadzone = new( 8, 8 );
	}
}
