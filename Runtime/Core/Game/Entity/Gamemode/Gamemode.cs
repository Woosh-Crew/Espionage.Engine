using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine.Gamemodes
{
	/// <summary>
	/// A Gamemode is where your game-flow happens. It controls how a game
	/// functions in a SOLID way.
	/// </summary>
	[Group( "Gamemodes" )]
	public abstract class Gamemode : Entity
	{
		public bool Persistant { get; set; }

		public bool Validate()
		{
			return OnValidation();
		}

		protected virtual bool OnValidation()
		{
			return true;
		}

		//
		// States
		//

		public void Begin()
		{
			Debugging.Log.Info( $"Starting Gamemode - [{ClassInfo.Title}]" );
			OnBegin();
		}

		protected virtual void OnBegin() { }

		public void Finish()
		{
			Debugging.Log.Info( $"Finishing Gamemode - [{ClassInfo.Title}]" );
			OnFinish();
		}

		protected virtual void OnFinish() { }

		// Client

		public void OnClientReady( Client client ) { }

		// Pawn

		public virtual void OnActorRespawned( Actor pawn ) { }
		public virtual bool OnActorDamaged( Actor pawn, ref IDamageable.Info info ) { return true; }
		public virtual void OnActorKilled( Actor pawn, IDamageable.Info info ) { }
	}
}
