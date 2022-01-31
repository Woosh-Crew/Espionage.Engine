using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	public class Pawn : Behaviour
	{
		public Tripod Tripod { get; protected set; }

		// Controller

		protected Controller Controller { get; set; }
		public Controller DevController { get; set; }

		public Controller GetActiveController()
		{
			return DevController ? DevController : Controller;
		}
	}
}
