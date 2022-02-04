using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Local stores meta about the local game state.
	/// Such as Client, their pawn, their name, etc.
	/// </summary>
	public static class Local
	{
		public static Client Client { get; set; }
		public static Pawn Pawn => Client.Pawn;
	}
}
