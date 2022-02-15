namespace Espionage.Engine.Tripods
{
	public class FPSTripod : Tripod
	{
		protected override void Frame()
		{
			Position = Local.Pawn.Controller.EyePos;
			Rotation = Local.Pawn.Controller.EyeRot;
		}
	}
}
