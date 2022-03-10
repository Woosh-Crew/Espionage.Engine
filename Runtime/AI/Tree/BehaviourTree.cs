using Espionage.Engine.Resources;

namespace Espionage.Engine.AI
{
	[Group( "AI" ), Path( "ai", "assets://AI/" )]
	public class BehaviourTree : Resource
	{
		public override string Identifier { get; }
	}
}
