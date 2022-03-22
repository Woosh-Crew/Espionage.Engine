using Espionage.Engine;
using UnityEngine;

[Title( "Sneaky Killer" )]
public class SneakyKiller : Game
{
	public SneakyKiller()
	{
		Loader = new( "Bum" );
	}

	public override void OnReady() { }
	public override void OnShutdown() { }
}
