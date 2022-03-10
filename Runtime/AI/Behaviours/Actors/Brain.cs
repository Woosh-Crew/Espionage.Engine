using System;
using UnityEngine;
using UnityEngine.AI;
using AI = Espionage.Engine.AI;

namespace Espionage.Engine.AI
{
	[Group( "AI" ), Title( "AI Brain" )]
	public class Brain : Component<Actor>, Pawn.ICallbacks, IThinkable
	{
		private Graph Logic { get; set; }

		protected override void OnAwake()
		{
			base.OnAwake();

			if ( !TryGetComponent( out _agent ) )
			{
				_agent = gameObject.AddComponent<NavMeshAgent>();
			}
		}

		protected override void OnReady()
		{
			base.OnReady();

			Logic = Graph.Load( graphPath );
			Entity.Tick = 0.2f;
		}

		public void Think( float delta )
		{
			_agent.destination = Local.Pawn.transform.position;

			Entity.Tick = 0.2f;
		}

		//
		// Possession
		//

		public void Possess( Client client )
		{
			Entity.Tick = 0;
		}

		public void UnPossess()
		{
			Entity.Tick = 0.2f;
		}

		// Fields

		private NavMeshAgent _agent;

		[SerializeField]
		private string graphPath;
	}
}
