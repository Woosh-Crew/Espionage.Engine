using System;
using UnityEngine;

namespace Espionage.Engine
{
	[Library( "info.player_start" )]
	public class SpawnPoint : Behaviour
	{
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere( transform.position, 0.25f );
			Gizmos.color = Color.white;
		}
	}
}
