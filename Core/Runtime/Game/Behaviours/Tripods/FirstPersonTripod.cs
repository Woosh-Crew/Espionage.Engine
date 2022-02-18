namespace Espionage.Engine
{
	public class FirstPersonTripod : Behaviour, ITripod
	{
		public void Activated( ref ITripod.Setup camSetup )
		{
			camSetup.Position = Local.Pawn.EyePos;
			camSetup.Rotation = Local.Pawn.EyeRot;
		}

		public void Deactivated() { }

		public void Build( ref ITripod.Setup camSetup )
		{
			if ( Local.Pawn == null )
			{
				return;
			}

			camSetup.Position = Local.Pawn.EyePos;
			camSetup.Rotation = Local.Pawn.EyeRot;
			camSetup.Viewer = Local.Pawn.gameObject;
		}
	}
}
