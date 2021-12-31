using System.Linq;
using System.Collections.Generic;

namespace Espionage.Engine.Internal.Commands
{
	public class SimpleCommandInvoker : ICommandInvoker
	{
		// Storage
		private Dictionary<string, Command> _commands;
		public IReadOnlyCollection<Command> All => _commands.Values;

		public void Add( Command command ) => _commands.Add( command.Name, command );

		public void Invoke( string command, string[] args )
		{
			if ( !_commands.TryGetValue( command, out var consoleCommand ) )
			{
				Debugging.Log.Info( $"Couldn't find command \"{command}\"" );
				return;
			}

			consoleCommand.Invoke( Command.ConvertArgs( consoleCommand.Info, args ) );
		}
	}
}
