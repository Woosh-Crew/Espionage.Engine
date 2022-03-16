using Espionage.Engine;
using Espionage.Engine.Resources;
using Espionage.Engine.Tripods;

[Title( "Sneaky Killer" )]
public class SneakyKiller : Game
{
	public override void OnReady() { }
	public override void OnShutdown() { }

	protected override void OnMapLoaded( Map map )
	{
		Dev.Terminal.Invoke( "map.current" );

		Local.Client.Pawn = Entity.Setup<Actor>()
			.With<FirstPersonTripod>()
			.With<FirstPersonController>()
			.Build();
	}
}
