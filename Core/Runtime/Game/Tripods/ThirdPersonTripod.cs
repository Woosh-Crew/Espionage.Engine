using UnityEngine;

namespace Espionage.Engine.Tripods
{
	public class ThirdPersonTripod : Tripod
	{
		public override void Activated( ref ITripod.Setup camSetup )
		{
			camSetup.Position = Local.Pawn.EyePos;
			camSetup.Rotation = Local.Pawn.EyeRot;
		}

		protected override void OnBuildTripod( ref ITripod.Setup camSetup )
		{
			if ( Local.Pawn == null )
			{
				return;
			}

			var pawn = Local.Pawn;

			// Set Rot first cause we use it below
			camSetup.Rotation = Quaternion.Slerp( camSetup.Rotation, pawn.EyeRot, Smoothing * Time.deltaTime );

			// Do a ray to calculate the distance, so we don't hit shit
			var ray = Physics.Raycast( new Ray( pawn.EyePos, camSetup.Rotation * Vector3.back ), out var hitInfo, Distance );
			var relativeOffset = camSetup.Rotation * Vector3.right * Offset.x + camSetup.Rotation * Vector3.up * Offset.y;

			camSetup.Position = Local.Pawn.EyePos + relativeOffset + camSetup.Rotation * Vector3.back * ((ray ? hitInfo.distance : Distance) - Padding);
		}

		protected override void OnBuildControls( ref IControls.Setup setup )
		{
			base.OnBuildControls( ref setup );
			setup.ViewAngles.x = Mathf.Clamp( setup.ViewAngles.x, -60, 88 );
		}

		// Fields

		[Property] public Vector2 Offset { get; set; }

		[Property] public float Smoothing { get; set; }

		[Property] public float Distance { get; set; }

		[Property] public float Padding { get; set; }
	}
}
