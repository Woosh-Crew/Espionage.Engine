using System.Linq;

namespace Espionage.Engine
{
	/// <summary>
	/// Actor is the gameplay pawn, where AI and Clients can Posses. AI will look
	/// for Actors instead of Pawns. Actors Also have Health, Respawning, Inventory
	/// and other gameplay specific things.
	/// </summary>
	public class Actor : Pawn
	{
		public Inventory Inventory => Components.Get<Inventory>();

		protected override void OnAwake()
		{
			base.OnAwake();

			Health.OnKilled += OnKilled;
			Health.OnDamaged += OnDamaged;
		}

		protected override void OnDelete()
		{
			base.OnDelete();

			Health.OnKilled -= OnKilled;
			Health.OnDamaged -= OnDamaged;
		}

		//
		// Health
		//

		public Health Health => Components.GetOrCreate( () => gameObject.AddComponent<Health>() );

		/// <summary>
		/// Respawns this Actor, and gives it max health. Use 
		/// this after you have possessed the pawn to make
		/// sure it spawns at a Spawn Point.
		/// </summary>
		public virtual void Respawn()
		{
			var health = Health;
			health.Heal( health.Max - health.Current );

			if ( Engine.Game.Gamemode != null )
			{
				// Tell the Gamemode, we want to respawn
				Engine.Game.Gamemode.OnActorRespawned( this );
			}

			foreach ( var item in Components.GetAll<ICallbacks>() )
			{
				item.Respawn();
			}
		}


		/// <summary>
		/// Called from the Health component when this Actor
		/// receives damaged. Return false, if you don't want to
		/// damage this actor.
		/// </summary>
		protected virtual bool OnDamaged( ref IDamageable.Info info )
		{
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
			foreach ( var item in Components.GetAll<ICallbacks>() )
			{
				item.OnKilled( info );
			}
		}

		//
		// AI
		//

		/// <summary>
		/// Sees if it has an AI Controller Attached to the actor
		/// and returns true or false depending on if it is null or active 
		/// </summary>
		public bool IsBot => !IsClient && Components.Has<AI.Brain>();

		//
		// Callbacks
		//

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
