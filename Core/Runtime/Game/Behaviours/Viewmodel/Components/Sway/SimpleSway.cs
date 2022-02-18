using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public sealed class SimpleSway : Behaviour, Viewmodel.IEffect
	{
		private Quaternion _lastSwayRot;
		private Vector3 _lastSwayPos;

		public void PostCameraSetup( ref ITripod.Setup setup )
		{
			var mouse = new Vector2( Input.GetAxisRaw( "Mouse X" ), Input.GetAxisRaw( "Mouse Y" ) );

			var trans = transform;
			_lastSwayRot = Quaternion.Slerp( _lastSwayRot, Quaternion.Euler( new Vector3( mouse.y, mouse.x, mouse.x ) * multiplier ), rotationDamping * Time.deltaTime );
			_lastSwayPos = Vector3.Lerp( _lastSwayPos, transform.localRotation * Vector3.up * mouse.y + trans.localRotation * Vector3.left * mouse.x, offsetDamping * Time.deltaTime );

			trans.localRotation *= _lastSwayRot;
			// trans.localPosition += _lastSwayPos;
		}

		// Fields

		[SerializeField]
		private float multiplier = 1;

		[SerializeField]
		private float rotationDamping = 4;

		[SerializeField]
		private float offsetDamping = 4;
	}
}
