using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Espionage.Engine.Internal.Commands
{
	/// <summary> Attribute Command Provider caches commands based off an attribute </summary>
	internal class AttributeCommandProvider : ICommandProvider
	{
		// Commands

		private readonly Dictionary<string, Command> _commands = new( StringComparer.CurrentCultureIgnoreCase );
		public IReadOnlyCollection<Command> All => _commands.Values;

		// History

		private readonly HashSet<string> _history = new();
		public IReadOnlyCollection<string> History => _history;

		//
		// Provider
		//

		public AttributeCommandProvider()
		{
			_commands ??= new Dictionary<string, Command>( StringComparer.CurrentCultureIgnoreCase );
			_commands.Clear();

			// Select all types where ILibrary exists or if it has the correct attribute
			for ( var assemblyIndex = 0; assemblyIndex < AppDomain.CurrentDomain.GetAssemblies().Length; assemblyIndex++ )
			{
				var assembly = AppDomain.CurrentDomain.GetAssemblies()[assemblyIndex];
				if ( !Utility.IgnoreIfNotUserGeneratedAssembly( assembly ) )
				{
					continue;
				}

				foreach ( var type in assembly.GetTypes() )
				{
					if ( Utility.IgnoredNamespaces.Any( e => e == type.Namespace ) )
					{
						continue;
					}

					foreach ( var member in type.GetMembers( BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic ) )
					{
						var attribute = member.GetCustomAttribute<Debugging.CmdAttribute>();

						if ( attribute is null )
						{
							continue;
						}

						foreach ( var item in attribute.Create( member ) )
						{
							Add( item );
						}
					}
				}
			}
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
