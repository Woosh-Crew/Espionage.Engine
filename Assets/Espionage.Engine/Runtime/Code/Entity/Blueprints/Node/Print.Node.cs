namespace Espionage.Engine.Entities.Nodes
{
	[Library( "node.log", Title = "Print Log", Help = "Logs a string to the console" )]
	public class Log : Node
	{
		[Input]
		public string Message { get; set; } = "Message";

		[Output]
		public string Formated => $"Logged: {Message}";

		[Input]
		public void Entry()
		{
			// Entry point into node

			Debugging.Log.Info( Message );

			// Exit point, continue nodes
			Exit();
		}

		[Output]
		public void Exit()
		{
			// Continue execution logic...
		}
	}
}
