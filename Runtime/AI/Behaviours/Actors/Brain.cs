namespace Espionage.Engine.AI
{
	[Group( "AI" ), Title( "AI Brain" )]
	public class Brain : Component<Actor>
	{
		private ActionTree Logic { get; set; }
	}
}
