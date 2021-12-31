using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Espionage.Engine.Internal.Commands;

namespace Espionage.Engine.Internal.Commands
{
	public interface ICommandProvider
	{
		Task Initialize() { return null; }

		void Invoke( string command, string[] args );

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
		var args = command.Substring( name.Length ).SplitArguments();

		provider.Invoke( name, args );
	}

	public static string[] Find( this ICommandProvider provider, string input )
	{
		return provider.All.Select( e => e.Name ).Where( e => e.StartsWith( input ) ).ToArray();
	}
}
