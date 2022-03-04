using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine.Gamemodes
{
	/// <summary>
	/// A Gamemode is where your game-flow happens. It controls how a game
	/// functions in a SOLID way.
	/// </summary>
	[Group( "Gamemodes" ), RequireComponent( typeof( World ) )]
	public abstract class Gamemode : Behaviour, IComponent<World>
	{
		void IComponent<World>.OnAttached( World item ) { }

		//
		// Validation
		//

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

			started.Invoke();
		}

		protected virtual void OnBegin() { }

		public void Finish()
		{
			Debugging.Log.Info( $"Finishing Gamemode - [{ClassInfo.Title}]" );
			OnFinish();

			finished.Invoke();
		}

		protected virtual void OnFinish() { }


		// Client

		public void OnClientReady( Client client ) { }

		// Pawn

		public bool OnPawnDamaged( Pawn pawn ) { return true; }
		public void OnPawnRespawned( Pawn pawn ) { }
		public void OnPawnKilled( Pawn pawn ) { }

		//
		// Callbacks
		//

		[SerializeField]
		private Output started;

		[SerializeField]
		private Output finished;
	}
}
