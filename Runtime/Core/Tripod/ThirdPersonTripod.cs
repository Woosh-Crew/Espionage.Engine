using UnityEngine;

namespace Espionage.Engine.Tripods
{
	[Library( "tripod.third_person" )]
	public class ThirdPersonTripod : Tripod
	{
		public Vector2 Offset { get; set; }
		public float Smoothing { get; set; } = 20;
		public float Distance { get; set; } = 5;
		public float Padding { get; set; } = 0.1f;

		// Logic

		public override void Activated( ref Setup camSetup )
		{
			camSetup.Position = Local.Pawn.EyePos;
			camSetup.Rotation = Local.Pawn.EyeRot;
		}

		protected override void OnBuildTripod( ref Setup camSetup )
		{
			if ( Local.Pawn == null )
			{
				return;
			}

			camSetup.Viewer = null;
			var pawn = Local.Pawn;

			// Set Rot first cause we use it below
			camSetup.Rotation = Quaternion.Slerp( camSetup.Rotation, pawn.EyeRot, Smoothing * Time.deltaTime );

			// Do a ray to calculate the distance, so we don't hit shit
			var ray = Physics.Raycast( new( pawn.EyePos, camSetup.Rotation * Vector3.back ), out var hitInfo, Distance );
			var relativeOffset = camSetup.Rotation * Vector3.right * Offset.x + camSetup.Rotation * Vector3.up * Offset.y;

			camSetup.Position = Local.Pawn.EyePos + relativeOffset + camSetup.Rotation * Vector3.back * ((ray ? hitInfo.distance : Distance) - Padding);
		}
	}
}
