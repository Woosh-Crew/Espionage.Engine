namespace Espionage.Engine
{
	public partial class Actor
	{
		[Function, Button, Title( "Kill Actor" )]
		internal void KillButton()
		{
			Dev.Log.Info( "This would kill the pawn" );
		}
		
		[Function, Button, Title( "Respawn Actor" )]
		internal void RespawnButton()
		{
			Dev.Log.Info( "This would respawn the pawn" );
		}
	}
}
