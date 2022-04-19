namespace Espionage.Engine.Gamemodes
{
	[Persistent]
	public class Singleplayer : Gamemode
	{
		protected override bool OnValidation()
		{
			// This would return true if we're in a 
			// client only instance... but theres no
			// networking just yet

			return true;
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			Delete();
		}

		public override void OnActorRespawned( Actor pawn )
		{
			pawn.MoveTo( All.Random<SpawnPoint>() );
			Debugging.Log.Info( "Moving to spawn point" );
		}

		public override void OnActorKilled( Actor pawn, IDamageable.Info info )
		{
			base.OnActorKilled( pawn, info );

			if ( pawn == Local.Client.Pawn )
			{
				// Wait 2 seconds and reload from last save
			}
		}
	}
}
