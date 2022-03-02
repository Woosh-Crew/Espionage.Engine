using UnityEngine;

namespace Espionage.Engine.Cameras
{
	public class ThirdPersonTripod : Tripod
	{
		public override void Activated( ref ITripod.Setup camSetup )
		{
			camSetup.Position = Local.Pawn.EyePos;
			camSetup.Rotation = Local.Pawn.EyeRot;
		}

		private Quaternion _smoothedRotation;
		private Quaternion _smoothedRotation2;
		private Vector3 _smoothPos;

		protected override void OnBuildTripod( ref ITripod.Setup camSetup )
		{
			if ( Local.Pawn == null )
			{
				return;
			}

			var offseted = camSetup.Rotation * Vector3.left * offset.x + camSetup.Rotation * Vector3.up * offset.y + camSetup.Rotation * Vector3.forward * offset.z;
			camSetup.Position = Local.Pawn.EyePos + camSetup.Rotation * Vector3.back * distance + offseted;

			// Offset

			_smoothedRotation = Quaternion.Slerp( _smoothedRotation, Local.Client.Pawn.EyeRot, smoothing * Time.deltaTime );
			camSetup.Rotation = _smoothedRotation;

			_smoothedRotation2 = Quaternion.Lerp( _smoothedRotation2, Visuals.transform.parent.rotation, 10 * Time.deltaTime );
			Visuals.transform.rotation = _smoothedRotation2;
		}

		// Fields

		[SerializeField]
		private float smoothing = 10;

		[SerializeField]
		private float distance = 5;

		[SerializeField]
		private Vector3 offset;
	}
}
