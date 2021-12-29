using System.Collections.Generic;
using System.Linq;
using Espionage.Engine.Internal;
using static Espionage.Engine.Console;

namespace Espionage.Engine.Internal
{
	public interface ICommandProvider
	{
		void Add( Command command );
		void Invoke( string command, string[] args );
		void LaunchArgs( string arg );

		IReadOnlyCollection<Command> All { get; }
		IReadOnlyCollection<string> History { get; }
	}
}

//
// Extensions
//

public static class ICommandProviderExtensions
{
	public static void Invoke( this ICommandProvider provider, string command )
	{
		var name = command.Split( ' ' ).First();
		var args = command.Substring( command.Length ).SplitArguments();

		provider.Invoke( name, args );
	}

	public static string[] Find( this ICommandProvider provider, string input )
	{
		return provider.All.Select( e => e.Name ).Where( e => e.StartsWith( input ) ).ToArray();
	}
}
