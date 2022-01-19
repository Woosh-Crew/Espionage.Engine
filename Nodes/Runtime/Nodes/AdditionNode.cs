namespace Espionage.Engine.Nodes
{
	[Library( "nodes.math.addition", Title = "Addition", Group = "Math" )]
	public class AdditionNode : Node
	{
		[Input]
		public float A { get; set; }

		[Input]
		public float B { get; set; }
		
		[Output]
		public float Result => A + B;
	}
}
