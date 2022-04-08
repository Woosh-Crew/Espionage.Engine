using System.Collections.Generic;
using System.Linq;
using Espionage.Engine;
using Espionage.Engine.Internal.Commands;

namespace Espionage.Engine.Internal.Commands
{
	public interface ICommandProvider
	{
		void Add( Command command );
		void Invoke( string command, string[] args );
		Command Get( string command );

		IEnumerable<Command> All { get; }
		IReadOnlyCollection<string> History { get; }
	}
}

//
// Extensions
//
namespace Espionage.Engine
{
	public static class CommandProviderExtensions
	{
		public static void Invoke( this ICommandProvider provider, string commandLine )
		{
			foreach ( var targetCommand in commandLine.Split( ';' ) )
			{
				var name = targetCommand.TrimStart().Split( ' ' ).First();
				var args = targetCommand.Substring( name.Length ).SplitArguments();

				// Invoke multiple commands at the same time
				provider.Invoke( name, args );
			}
		}

		public static IEnumerable<Command> Find( this ICommandProvider provider, string input )
		{
			return provider.All.Where( e => e.Name.StartsWith( input ) );
		}
	}
}
