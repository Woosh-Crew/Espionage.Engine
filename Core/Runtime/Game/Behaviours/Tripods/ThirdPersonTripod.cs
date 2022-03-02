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

			// Set Rot first cause we use it below
			camSetup.Rotation = Local.Client.Pawn.EyeRot;

			var ray = Physics.Raycast( new Ray( Local.Pawn.EyePos, camSetup.Rotation * Vector3.back ), out var hitInfo, distance );

			if ( ray )
			{
				Debug.DrawLine( Local.Pawn.EyePos, hitInfo.point );
			}

			camSetup.Position = Local.Pawn.EyePos + camSetup.Rotation * Vector3.back * (ray ? hitInfo.distance : distance);
		}

		// Fields

		[SerializeField]
		private float distance = 5;
	}
}
