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

	protected override Map.Builder[] Maps => new[]
	{
		// Testing Lab		
		Map.Setup( "maps://map_testing.umap" )
			.Meta( "Testing Map", "Used for testing Espionage.Engine functionality.", "JakeSayingWoosh" )
			.Thumbnail( "textures://map.png" )
			.Origin( "Espionage" ),

		// Testing Lab 2
		Map.Setup( 0 )
			.Meta( "Testing 2" )
			.Origin( "Espionage" )
	};
}
