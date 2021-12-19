using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Espionage.Engine;
using Mirror;

[Library( "entity.test" )]
public class TestEntity : Entity
{
	[Net]
	public int SyncTest;

	public override void Spawn()
	{
		base.Spawn();

		SyncTest = 10;
	}
}
