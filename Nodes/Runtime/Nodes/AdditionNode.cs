namespace Espionage.Engine.Nodes
{
	[Library( "nodes.math.addition" )]
	public class AdditionNode : Node
	{
		[Input] public float A { get; set; }

		[Input] public float B { get; set; }

		[Output] public float Result => A + B;
	}
}
