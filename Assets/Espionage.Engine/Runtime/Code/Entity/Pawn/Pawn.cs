using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	public class Pawn : Entity
	{
		//
		// Think
		//

		// protected override void Update()
		// {

		// 	// Check if we can die, This is stupid 
		// 	if ( this is IDamageable damageable && damageable.Dead )
		// 		return;

		// 	GetActiveController()?.Think( Time.deltaTime );
		// }

		//
		// Pawn States
		//

		public virtual void Possess() { }

		public virtual void UnPossess() { }

		public virtual void Respawn() { }

		//
		// Controller
		//

		public Controller Controller { get; set; }
		public Controller DevController { get; set; }

		public Controller GetActiveController()
		{
			return DevController ?? Controller;
		}
	}
}
