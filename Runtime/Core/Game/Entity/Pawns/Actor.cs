namespace Espionage.Engine
{
	/// <summary>
	/// Actor is the gameplay pawn, where AI and Clients can Posses. AI will look
	/// for Actors instead of Pawns. Actors Also have Health, Respawning, Inventory
	/// and other gameplay specific things.
	/// </summary>
	[Help( "Actor is designed to be a pawn used in gameplay, AI and Clients control this. AI will look for Actors instead of pawns is well." )]
	public class Actor : Pawn
	{
		public bool IsBot => Components.Get<AI.Brain>()?.Enabled ?? false;

		public override void Spawn()
		{
			base.Spawn();
			
			Health.OnKilled += OnKilled;
			Health.OnDamaged += OnDamaged;
		}

		protected override void OnDelete()
		{
			Health.OnKilled -= OnKilled;
			Health.OnDamaged -= OnDamaged;
		}

		//
		// Health
		//

		public Health Health => Components.GetOrCreate( () => new Health() { Max = 100 } );

		/// <summary>
		/// Respawns this Actor, and gives it max health. Use 
		/// this after you have possessed the pawn to make
		/// sure it spawns at a Spawn Point.
		/// </summary>
		/// <returns> Returns this Actor </returns>
		[Function, Button]
		public virtual Actor Respawn()
		{
			Debugging.Log.Info( "Respawning Actor" );

			var health = Health;
			health.Heal( health.Max - health.Current );

			if ( Engine.Game.Gamemode != null )
			{
				// Tell the Gamemode, we want to respawn
				Engine.Game.Gamemode.OnActorRespawned( this );
			}
			else
			{
				// Just move to random Spawn point
				this.MoveTo( All.Random<SpawnPoint>() );
			}

			foreach ( var item in Components.GetAll<ICallbacks>() )
			{
				item.Respawn();
			}

			return this;
		}

		[Function, Button]
		public void Kill()
		{
			Health.Damage( Health.Max );
		}

		/// <summary>
		/// Called from the Health component when this Actor
		/// receives damaged. Return false, if you don't want to
		/// damage this actor.
		/// </summary>
		protected virtual bool OnDamaged( ref IDamageable.Info info )
		{
			// Ask Gamemode if we can Damage
			if ( Engine.Game.Gamemode != null && !Engine.Game.Gamemode.OnActorDamaged( this, ref info ) )
			{
				return false;
			}

			// Ask Components if we can Damage
			foreach ( var item in Components.GetAll<ICallbacks>() )
			{
				// Can this be damaged?
				if ( !item.OnDamaged( ref info ) )
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Called from the Health component when this Actor
		/// is killed.
		/// </summary>
		protected virtual void OnKilled( IDamageable.Info info )
		{
			Debugging.Log.Info( "Actor got Killed" );

			if ( Engine.Game.Gamemode != null )
			{
				// Tell the Gamemode, we want to respawn
				Engine.Game.Gamemode.OnActorKilled( this, info );
			}

			foreach ( var item in Components.GetAll<ICallbacks>() )
			{
				item.OnKilled( info );
			}
		}

		/// <summary>
		/// Component Callbacks Specific for this Entity.
		/// Use this interface on an Actor component if you
		/// wanna have actor specific callbacks.
		/// </summary>
		public new interface ICallbacks
		{
			/// <inheritdoc cref="Actor.Respawn"/>
			void Respawn();

			/// <inheritdoc cref="Actor.OnDamaged"/>
			bool OnDamaged( ref IDamageable.Info info );

			/// <inheritdoc cref="Actor.OnKilled"/>
			void OnKilled( IDamageable.Info info );
		}

	}
}
