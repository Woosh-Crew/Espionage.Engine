using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public sealed class SimpleSway : Behaviour, Viewmodel.IEffect
	{
		private Vector2 _lastDelta;
		private Quaternion _lastSwayRot;
		private Vector3 _lastSwayPos;

		public void PostCameraSetup( ref ITripod.Setup setup )
		{
			// I have no clue wtf this does, but apparently me from the past did
			_lastDelta = Vector2.Lerp( _lastDelta, (new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) / Time.deltaTime) / 3.5f, 45 * Time.deltaTime );

			var mouse = _lastDelta;
			mouse *= multiplier;

			var trans = transform;
			_lastSwayRot = Quaternion.Slerp( _lastSwayRot, Quaternion.Euler( mouse.y, -mouse.x * 2, mouse.x ), 6 * Time.deltaTime );
			_lastSwayPos = Vector3.Lerp( _lastSwayPos, transform.localRotation * Vector3.up * mouse.y / 2 + trans.localRotation * Vector3.left * mouse.x / 2, 6 * Time.deltaTime );

			trans.localRotation *= _lastSwayRot;
			trans.localPosition += _lastSwayPos;
		}
		
		// Fields
		
		[SerializeField]
		private float multiplier;
	}
}
