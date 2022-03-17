using Espionage.Engine;
using Espionage.Engine.Resources;
using Espionage.Engine.Tripods;
using Espionage.Engine.UI;

[Title( "Sneaky Killer" )]
public class SneakyKiller : Game
{
	public override void OnReady()
	{
		Library.Database.Create<TerminalUI>();
	}

	public override void OnShutdown() { }
}
