using System;
using System.Collections.Generic;
using UnityEngine;
using static Espionage.Engine.Console;

namespace Espionage.Engine.Internal
{
	public class CommandProvider : ICommandProvider
	{
		// Commands
		private static Dictionary<string, Command> _commands;
		public IReadOnlyCollection<Command> All => _commands.Values;

		// History
		private static HashSet<string> _history = new HashSet<string>();
		public IReadOnlyCollection<string> History => _history;

		public void Add( Command command )
		{
			_commands ??= new Dictionary<string, Command>( StringComparer.CurrentCultureIgnoreCase );

			try
			{
				_commands.Add( command.Name, command );
			}
			catch ( Exception e )
			{
				Debug.LogException( e );
			}
		}

		public void Invoke( string command, string[] args )
		{
			if ( !_commands.TryGetValue( command, out var consoleCommand ) )
			{
				Debug.Log( $"Couldn't find command \"{command}\"" );
				return;
			}

			// Check if we are on the correct layer - This looks ultra aids
			if ( (Application.isEditor && consoleCommand.Layer.HasFlag( Layer.Editor )) || (Application.isPlaying && consoleCommand.Layer.HasFlag( Layer.Runtime )) )
			{
				if ( args is not null && args.Length > 0 )
					consoleCommand.Invoke( ConvertArgs( GetParameterTypes( consoleCommand.Info ), args ) );
				else
					consoleCommand.Invoke( null );

				return;
			}

			Debug.Log( $"Trying to invoke command on wrong layer [{consoleCommand.Layer}]" );
			return;
		}
	}
}
