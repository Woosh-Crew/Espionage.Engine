using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	public class Controller : Behaviour, IComponent<Pawn>
	{
		public void OnAttached( Pawn item ) { }
	}
}
