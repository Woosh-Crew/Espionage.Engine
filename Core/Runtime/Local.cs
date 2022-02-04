using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	public static class Local
	{
		public static Client Client { get; set; }
		public static Pawn Pawn => Client.Pawn;
	}
}
