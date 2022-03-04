namespace Espionage.Engine.Tripods
{
	[Library( "tripod.first_person" )]
	public class FirstPersonTripod : Tripod
	{
		public override void Activated( ref ITripod.Setup camSetup )
		{
			camSetup.Rotation = Local.Pawn.EyeRot;
			camSetup.Position = Local.Pawn.EyePos;
		}

		protected override void OnBuildTripod( ref ITripod.Setup camSetup )
		{
			if ( Local.Pawn == null )
			{
				return;
			}

			camSetup.Rotation = Local.Pawn.EyeRot;
			camSetup.Position = Local.Pawn.EyePos;
		}
	}
}
