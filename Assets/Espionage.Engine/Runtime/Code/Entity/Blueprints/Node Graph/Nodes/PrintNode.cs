namespace Espionage.Engine.Entities.Nodes
{
	[Library( "node.log", Title = "Print Log", Help = "Logs a string to the console" )]
	public class PrintNode : Node
	{
		public void Entry()
		{
		}

		protected override void OnBuildInputs()
		{
			base.OnBuildInputs();

			// Build Input nodes...
			// Such as the entry point
			// and variables
		}

		protected override void OnBuildOutputs()
		{
			base.OnBuildOutputs();

			// Build output nodes
			// Send out variables
			// Or continue the tree
		}
	}
}
