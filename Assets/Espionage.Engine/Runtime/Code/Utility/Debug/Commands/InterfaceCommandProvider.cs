using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace Espionage.Engine.Internal
{
	/// <summary> Uses reflection to get all types that use the ICommandCreator and creates commands 
	/// based off its Create() method, doesnt cache any class that inherits from Attribute </summary>
	public class InterfaceCommandProvider : ICommandProvider
	{
		// Commands
		private static Dictionary<string, Command> _commands = new Dictionary<string, Command>( StringComparer.CurrentCultureIgnoreCase );
		public IReadOnlyCollection<Command> All => _commands.Values;

		// History
		private static HashSet<string> _history = new HashSet<string>();
		public IReadOnlyCollection<string> History => _history;


		public Task Initialize()
		{
			return Task.Run( () =>
				{
					// Get every CmdAttribute using Linq
					var types = AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany( e => e.GetTypes()
										.Where( e => !e.IsAbstract && (e.GetInterfaces().Contains( typeof( ICommandCreator ) )) && !e.IsSubclassOf( typeof( Attribute ) ) ) );

					foreach ( var info in types )
					{
						// Create an instance of that class so we can invoke its Create method
						var commands = Activator.CreateInstance( info ) as ICommandCreator;
						foreach ( var item in commands.Create( info ) )
						{
							Add( item );
						}
					}
				} );
		}

		public void Add( Command command )
		{
			try
			{
				_commands.Add( command.Name, command );
			}
			catch ( Exception e )
			{
				Debug.LogException( e );
			}
		}

		public void Remove( string name )
		{
			try
			{
				_commands.Remove( name );
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

			// Add to history stack, for use later
			_history.Add( $"{command} {string.Join( ' ', args )}" );

			// Check if we are on the correct layer - This looks ultra aids
			if ( args is not null && args.Length > 0 )
				consoleCommand.Invoke( Command.ConvertArgs( consoleCommand.Info, args ) );
			else
				consoleCommand.Invoke( null );
		}

		public void LaunchArgs( string arg )
		{

		}
	}
}
