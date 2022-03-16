using System;
using System.Collections.Generic;

namespace Espionage.Engine.Internal.Commands
{
	internal class SimpleCommandProvider : ICommandProvider
	{
		// Commands
		private readonly Dictionary<string, Command> _commands = new( StringComparer.CurrentCultureIgnoreCase );


		public IEnumerable<Command> All => _commands.Values;

		// History
		private readonly HashSet<string> _history = new();
		public IReadOnlyCollection<string> History => _history;

		//
		// Provider
		//

		public SimpleCommandProvider()
		{
			_commands ??= new( StringComparer.CurrentCultureIgnoreCase );
			_commands.Clear();
		}

		public void Add( Command command )
		{
			_commands.Add( command.Name, command );
		}

		public Command Get( string command )
		{
			return _commands.ContainsKey( command ) ? _commands[command] : default;
		}

		public void Invoke( string command, string[] args )
		{
			if ( !_commands.TryGetValue( command, out var consoleCommand ) )
			{
				Dev.Log.Info( $"Couldn't find command \"{command}\"" );
				return;
			}

			consoleCommand.Invoke( Command.ConvertArgs( consoleCommand.Info, args ) );

			// Add to history stack, for use later
			_history.Add( $"{command} {string.Join( ' ', args )}" );
		}

		public void LaunchArgs( string arg ) { }
	}
}
