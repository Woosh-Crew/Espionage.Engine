using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	public static class Local
	{
		public static Client Client { get; internal set; }

		// Client Helpers
		public static Pawn Pawn => Client.Pawn;
		public static Tripod.Stack Tripod => Client.Tripod;
	}
}
