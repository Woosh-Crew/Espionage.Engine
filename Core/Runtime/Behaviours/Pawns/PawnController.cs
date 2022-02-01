using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	public abstract class PawnController : Behaviour, IComponent<Pawn>
	{
		void IComponent<Pawn>.OnAttached( Pawn item ) { }

		public virtual void Simulate() { }
	}
}
