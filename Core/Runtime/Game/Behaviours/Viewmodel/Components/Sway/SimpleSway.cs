using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public sealed class SimpleSway : Behaviour, Viewmodel.IEffect
	{
		private Quaternion _targetSwayRot;
		private Quaternion _lastSwayRot;

		private Vector3 _lastSwayPos;

		public void PostCameraSetup( ref ITripod.Setup setup )
		{
			var mouse = new Vector2(
				Input.GetAxisRaw( "Mouse X" ) * rotationMultiplier.x,
				Input.GetAxisRaw( "Mouse Y" ) * rotationMultiplier.y
			);

			var trans = transform;

			// calculate target rotation
			var rotationX = Quaternion.AngleAxis( -mouse.y, Vector3.right );
			var rotationY = Quaternion.AngleAxis( mouse.x, Vector3.up );

			trans.rotation *= rotationX * rotationY;
		}

		// Fields

		[Header( "Rotation" ), SerializeField]
		private Vector3 rotationMultiplier = new( 6, 1, 4 );

		[SerializeField]
		private float rotationDamping = 6;

		[Header( "Offset" ), SerializeField]
		private Vector2 offsetMultiplier = new( 0.1f, 0.05f );

		[SerializeField]
		private float offsetDamping = 3;
	}
}
