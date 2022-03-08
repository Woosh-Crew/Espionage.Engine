using System;
using Espionage.Engine.Tripods;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Actor is the gameplay pawn, where AI
	/// and Clients can Posses. AI will look
	/// for Actors instead of Pawns. Actors
	/// Also have Health, Respawning, Inventory
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
		}

		protected override void OnDelete()
		{
			base.OnDelete();

			Health.OnKilled -= OnKilled;
		}

		public virtual void Respawn()
		{
			if ( Health != null )
			{
				Health.Heal( 100 );
			}

			if ( Engine.Game.Gamemode != null )
			{
				// Tell the Gamemode, we want to respawn
				Engine.Game.Gamemode.OnPawnRespawned( this );
			}
		}

		protected virtual void OnKilled() { }

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
