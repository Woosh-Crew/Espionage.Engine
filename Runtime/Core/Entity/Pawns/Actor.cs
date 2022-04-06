using System.Linq;

namespace Espionage.Engine
{
	/// <summary>
	/// Actor is the gameplay pawn, where AI and Clients can Posses. AI will look
	/// for Actors instead of Pawns. Actors Also have Health, Respawning, Inventory
	/// and other gameplay specific things.
	/// </summary>
	[Help( "Actor is designed to be a pawn used in gameplay, AI and Clients control this. AI will look for Actors instead of pawns is well." )]
	public abstract class Actor : Pawn
	{
		public Inventory Inventory => Components.Get<Inventory>();
		public bool IsBot => Components.Get<AI.Brain>()?.Enabled ?? false;

		protected override void OnAwake()
		{
			base.OnAwake();

			Health.OnKilled += OnKilled;
			Health.OnDamaged += OnDamaged;
		}

		protected override void OnDelete()
		{
			Health.OnKilled -= OnKilled;
			Health.OnDamaged -= OnDamaged;
		}

		public override void PostCameraSetup( ref Tripod.Setup setup )
		{
			var holdable = Inventory as IHasHoldable;
			if ( holdable != null && holdable.Active != null )
			{
				holdable.Active.PostCameraSetup( ref setup );
			}

			base.PostCameraSetup( ref setup );
		}

		//
		// Health
		//

		public Health Health => Components.GetOrCreate( () =>
		{
			var comp = Library.Database.Create<Health>();
			comp.Max = 100;
			return comp;
		} );

		/// <summary>
		/// Respawns this Actor, and gives it max health. Use 
		/// this after you have possessed the pawn to make
		/// sure it spawns at a Spawn Point.
		/// </summary>
		/// <returns> Returns this object </returns>
		public virtual Actor Respawn()
		{
			var health = Health;
			health.Heal( health.Max - health.Current );

			if ( Engine.Game.Gamemode != null )
			{
				// Tell the Gamemode, we want to respawn
				Engine.Game.Gamemode.OnActorRespawned( this );
			}
			else
			{
				// Just Apply Spawn point
				var spawn = All.OfType<SpawnPoint>().Random();
				if ( spawn != null )
				{
					Position = spawn.Position;
					Rotation = spawn.Rotation;
				}
			}

			foreach ( var item in Components.GetAll<ICallbacks>() )
			{
				item.Respawn();
			}

			return this;
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
