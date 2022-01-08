using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Espionage.Engine.Internal.Commands
{
	/// <summary> Attribute Command Provider caches commands based off an attribute </summary>
	/// <typeparam name="T"> Attribute, should also have interface ICommandCreator </typeparam>
	internal class AttributeCommandProvider<T> : ICommandProvider where T : Attribute
	{
		//
		// Commands
		private Dictionary<string, Command> _commands = new Dictionary<string, Command>( StringComparer.CurrentCultureIgnoreCase );
		public IReadOnlyCollection<Command> All => _commands.Values;

		//
		// History
		private static HashSet<string> _history = new HashSet<string>();
		public IReadOnlyCollection<string> History => _history;

		//
		// Provider
		//

		public Task Initialize()
		{
			return Task.Run( () =>
				{
					_commands ??= new Dictionary<string, Command>( StringComparer.CurrentCultureIgnoreCase );
					_commands.Clear();

					// Get every CmdAttribute using Linq
					var types = AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany( e => e.GetTypes()
										.SelectMany( e => e.GetMembers( BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic )
										.Where( e => e.IsDefined( typeof( T ) ) ) ) );

					foreach ( var info in types )
					{
						foreach ( var item in (info.GetCustomAttribute<T>() as ICommandCreator).Create( info ) )
							Add( item );
					}
				} );
		}

		public void Add( Command command ) => _commands.Add( command.Name, command );

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

		public void LaunchArgs( string arg )
		{

		}
	}
}
