namespace Espionage.Engine.Gamemodes
{
	public abstract class Singleplayer : Gamemode
	{
		protected override bool OnValidation()
		{
			// This would return true if we're in a 
			// client only instance... but theres no
			// networking just yet

			return true;
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
