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
			mouse.Normalize();

			var trans = transform;
			_lastSwayRot = Quaternion.Slerp( _lastSwayRot, Quaternion.Euler( new Vector3( mouse.y, mouse.x, mouse.x ) * rotationMultiplier ), rotationDamping * Time.deltaTime );
			_lastSwayPos = Vector3.Lerp( _lastSwayPos, transform.localRotation * Vector3.up * mouse.y + trans.localRotation * Vector3.left * mouse.x, offsetDamping * Time.deltaTime );

			// For some reason local rotation..
			// makes it stutter hard? Makes no sense
			trans.rotation *= _lastSwayRot;
			trans.position += _lastSwayPos.normalized * offsetMultiplier;
		}

		// Fields

		[Header( "Rotation" ), SerializeField]
		private float rotationMultiplier = 1;

		[SerializeField]
		private float rotationDamping = 4;

		[Header( "Offset" ), SerializeField]
		private float offsetMultiplier = 1;

		[SerializeField]
		private float offsetDamping = 4;
	}
}
