using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Espionage.Engine.AI
{
	[Group( "AI" ), Title( "AI Brain" )]
	public class Brain : Component<Actor>, Pawn.ICallbacks, Actor.ICallbacks
	{
		public bool Enabled { get; private set; }

		protected override void OnAttached( Actor actor )
		{
			if ( !actor.TryGetComponent( out _agent ) )
			{
				_agent = actor.gameObject.AddComponent<NavMeshAgent>();
			}

			Entity.Thinking.Add( Think, 0.2f );
			Senses = Entity.Components.GetAll<Sense>().ToArray();
		}

		private void Think()
		{
			var randomDirection = Random.insideUnitSphere * 35;
			NavMesh.SamplePosition( randomDirection, out var hit, 35, 1 );
			_agent.SetDestination( hit.position );

			Entity.Tick = Random.Range( 1, 8 );
		}

		// AI

		private NavMeshAgent _agent;

		// Senses

		public Sense[] Senses { get; private set; }

		// Actor Health

		public void Respawn()
		{
			// We, don't care if this pawn respawns
			// Cause it gets destroyed when it dies.
		}

		public bool OnDamaged( ref IDamageable.Info info ) { return true; }

		public void OnKilled( IDamageable.Info info ) { }

		// Pawn Possession

		void Pawn.ICallbacks.Possess( Client client )
		{
			Enabled = false;

			_agent.enabled = false;
			Entity.Thinking.Remove( Think );
		}

		void Pawn.ICallbacks.UnPossess()
		{
			Enabled = true;

			_agent.enabled = true;
			Entity.Thinking.Add( Think, 0.2f );
		}
	}
}
