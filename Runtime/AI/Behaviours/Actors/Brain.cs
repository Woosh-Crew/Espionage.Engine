using UnityEngine;
using UnityEngine.AI;
using AI = Espionage.Engine.AI;

namespace Espionage.Engine.AI
{
	[Group( "AI" ), Title( "AI Brain" )]
	public class Brain : Component<Actor>, Pawn.ICallbacks
	{
		private Graph Logic { get; set; }

		protected override void OnAttached( Actor actor )
		{
			base.OnAwake();

			if ( !TryGetComponent( out _agent ) )
			{
				_agent = gameObject.AddComponent<NavMeshAgent>();
			}

			Entity.Thinking.Add( Think, 0.2f );
		}

		public void Think()
		{
			_agent.destination = Local.Pawn.transform.position;

			Entity.Tick = 0.2f;
		}

		//
		// Possession
		//

		public void Possess( Client client )
		{
			Entity.Thinking.Remove( Think );
		}

		public void UnPossess()
		{
			Entity.Thinking.Add( Think, 0.2f );
		}

		// Fields

		private NavMeshAgent _agent;

		[SerializeField]
		private string graphPath;
	}
}
