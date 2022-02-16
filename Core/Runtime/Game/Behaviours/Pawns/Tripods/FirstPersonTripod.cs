namespace Espionage.Engine
{
	public class FirstPersonTripod : Tripod
	{
		public override void Build( ref ICamera.Setup camSetup )
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
