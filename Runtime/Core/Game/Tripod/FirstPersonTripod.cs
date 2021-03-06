using UnityEngine;

namespace Espionage.Engine.Tripods
{
	[Library( "tripod.first_person" )]
	public class FirstPersonTripod : Tripod
	{
		public float NeckLength { get; set; } = 0.28f;

		// Logic

		public override void Activated( ref Setup camSetup )
		{
			camSetup.Rotation = Local.Pawn.Eyes.Rotation;
			camSetup.Position = Local.Pawn.Eyes.Position;
		}

		protected override void OnBuildTripod( ref Setup camSetup )
		{
			if ( Local.Pawn == null )
			{
				return;
			}

			camSetup.Viewer = Entity;

			camSetup.Rotation = Local.Pawn.Eyes.Rotation;
			camSetup.Position = Local.Pawn.Eyes.Position;

			var pitch = camSetup.Rotation.Pitch() - (camSetup.Rotation.Pitch() > 270 ? 360 : 0);
			camSetup.Position += Local.Pawn.Rotation * Vector3.forward * pitch.Remap( -90, 90, -NeckLength * Local.Pawn.Scale.magnitude, NeckLength * Local.Pawn.Scale.magnitude );
		}

		protected override void OnBuildControls( Controls.Setup setup )
		{
			// Only rotate if the cursor is locked.

			if ( setup.Cursor.Locked )
			{
				base.OnBuildControls( setup );
			}
		}
	}
}
