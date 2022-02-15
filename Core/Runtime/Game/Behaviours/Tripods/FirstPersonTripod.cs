namespace Espionage.Engine.Tripods
{
	public class FirstPersonTripod : Tripod
	{
		protected override void Frame()
		{
			if ( Local.Pawn == null )
			{
				return;
			}

			Position = Local.Pawn.EyePos;
			Rotation = Local.Pawn.EyeRot;
			Viewer = Local.Pawn.gameObject;
		}
	}
}
