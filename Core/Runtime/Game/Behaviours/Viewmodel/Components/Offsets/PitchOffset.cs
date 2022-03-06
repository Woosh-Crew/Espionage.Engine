using UnityEngine;

namespace Espionage.Engine.Pickups.Viewmodels
{
	public class PitchOffset : Viewmodel.Effect
	{
		protected Quaternion lastOffsetRot;
		protected Vector3 lastOffsetPos;

		public override void PostCameraSetup( ref ITripod.Setup setup )
		{
			var offset = setup.Rotation.eulerAngles.x.Remap( -90, 90, -1, 1 );

			lastOffsetRot = Quaternion.Slerp( lastOffsetRot, Quaternion.Euler( offset * pitchOffset, 0, 0 ), damping * Time.deltaTime );
			lastOffsetPos = Vector3.Lerp( lastOffsetPos, transform.rotation * Vector3.up * offset * axisOffset, damping * Time.deltaTime );

			transform.rotation *= lastOffsetRot;
			transform.position += lastOffsetPos;
		}

		// Fields

		[SerializeField]
		private float damping = 15;

		[SerializeField]
		private float axisOffset = 4;

		[SerializeField]
		private float pitchOffset = 5;
	}
}
