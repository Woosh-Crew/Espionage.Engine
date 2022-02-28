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

		private Vector3 _smoothedPosition;

		protected override void OnBuildTripod( ref ITripod.Setup camSetup )
		{
			if ( Local.Pawn == null )
			{
				return;
			}

			var offseted = camSetup.Rotation * Vector3.left * offset.x + camSetup.Rotation * Vector3.up * offset.y + camSetup.Rotation * Vector3.forward * offset.z;
			_smoothedPosition = Vector3.Slerp( _smoothedPosition, Local.Pawn.EyePos + camSetup.Rotation * Vector3.back * distance + offseted, smoothing * Time.deltaTime );
			camSetup.Position = _smoothedPosition;

			// Offset

			camSetup.Rotation = Local.Pawn.EyeRot;
		}

		// Fields

		[SerializeField]
		private float smoothing = 15;

		[SerializeField]
		private float distance = 5;

		[SerializeField]
		private Vector3 offset;
	}
}
