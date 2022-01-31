using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	public class Pawn : Behaviour
	{
		public Tripod Tripod { get; protected set; }

		// Controller

		protected PawnController PawnController { get; set; }
		public PawnController DevPawnController { get; set; }

		public PawnController GetActiveController()
		{
			return DevPawnController ? DevPawnController : PawnController;
		}
	}
}
