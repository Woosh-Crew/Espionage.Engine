namespace Espionage.Engine
{
	public partial class Pawn
	{
		//
		// Buttons
		//

		[Function, Button, Title( "Possess Pawn" )]
		internal void PossessButton()
		{
			Local.Client.Pawn = this;
			Dev.Terminal.Invoke( "dev.tripod" );
		}

		[Function, Button, Title( "Kill Pawn" )]
		internal void KillButton()
		{
			Dev.Log.Info( "This would kill the pawn" );
		}
	}
}
