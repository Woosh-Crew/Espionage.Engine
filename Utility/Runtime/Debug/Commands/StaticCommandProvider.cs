using System;
using System.Collections.Generic;

namespace Espionage.Engine.Internal.Commands
{
	/// <summary>Attribute provider where the commands are hardcoded.</summary>
	public class StaticCommandProvider : ICommandProvider
	{
		// Commands

		private readonly Dictionary<string, Command> _commands = new( StringComparer.CurrentCultureIgnoreCase );
		public IEnumerable<Command> All => _commands.Values;

		// History

		private readonly HashSet<string> _history = new();
		public IReadOnlyCollection<string> History => _history;

		public StaticCommandProvider()
		{
			// We will have a codegen provider
			// that will inherit from static command provider
			// and just fill up a constructor with the commands
			// from the editor version of the project.
		}
		
		public void Add( Command command )
		{
			_commands.Add( command.Name, command );
		}

		public void Invoke( string command, string[] args )
		{
			if ( !_commands.TryGetValue( command, out var consoleCommand ) )
			{
				Debugging.Log.Info( $"Couldn't find command \"{command}\"" );
				return;
			}

			consoleCommand.Invoke( Command.ConvertArgs( consoleCommand.Info, args ) );

			// Add to history stack, for use later
			_history.Add( $"{command} {string.Join( ' ', args )}" );
		}

		public void LaunchArgs( string arg ) { }
	}
}
