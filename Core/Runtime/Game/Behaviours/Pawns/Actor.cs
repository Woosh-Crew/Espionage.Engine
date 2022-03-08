﻿using System;
using Espionage.Engine.Tripods;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Actor is the gameplay pawn, where AI and Clients can Posses. AI will look
	/// for Actors instead of Pawns. Actors Also have Health, Respawning, Inventory
	/// and other gameplay specific things.
	/// </summary>
	[Group( "Pawns" )]
	public class Actor : Pawn, IControls
	{
		public Health Health => Components.GetOrCreate( () => gameObject.AddComponent<Health>() );
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

		public virtual void Respawn()
		{
			var health = Health;
			health.Heal( health.Max - health.Current );

			if ( Engine.Game.Gamemode != null )
			{
				// Tell the Gamemode, we want to respawn
				Engine.Game.Gamemode.OnActorRespawned( this );
			}
		}

		protected virtual void OnKilled( IDamageable.Info info ) { }
		protected virtual bool OnDamaged( IDamageable.Info info ) { return true; }


		void IControls.Build( ref IControls.Setup setup ) { }

		//
		// AI
		//

		/// <summary>
		/// Sees if it has an AI Controller Attached to the actor
		/// and returns true or false depending on if it is null or active 
		/// </summary>
		public bool IsBot => throw new NotImplementedException();

		/// <summary>
		/// Sees if it has an controller that can be used by a client,
		/// and returns true or false depending on if it is null or active 
		/// </summary>
		public bool IsClient => throw new NotImplementedException();
	}
}
