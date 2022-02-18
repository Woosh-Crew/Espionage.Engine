using Espionage.Engine.Gamemodes;
using UnityEngine;

namespace Espionage.Engine
{
	[RequireComponent( typeof( Health ) )]
	public class Actor : Pawn, IControls
	{
		public Health Health { get; private set; }

		protected override void OnAwake()
		{
			// Health
			Health = GetComponent<Health>();
		}

		public virtual void Respawn()
		{
			Health.Heal( 100 );

			if ( Engine.Game.Gamemode != null )
			{
				// Tell the Gamemode, we want to respawn
				Engine.Game.Gamemode.OnPawnRespawned( this );
			}
		}

		void IControls.Build( ref IControls.Setup setup ) { }
	}
}
