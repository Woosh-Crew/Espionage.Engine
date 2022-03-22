namespace Espionage.Engine
{
	public partial class Pawn
	{
		[Function, Button, Title( "Possess Pawn" )]
		internal void PossessButton()
		{
			Local.Client.Pawn = this;

			// Disable Dev Tripod
			Dev.Terminal.Invoke( "dev.tripod" );
		}
	}
}
