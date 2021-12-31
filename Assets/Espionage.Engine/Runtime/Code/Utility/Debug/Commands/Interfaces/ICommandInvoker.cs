using System.Collections.Generic;
using System.Reflection;

namespace Espionage.Engine.Internal.Commands
{
	public interface ICommandInvoker
	{
		IReadOnlyCollection<Command> All { get; }

		void Invoke( string command, string[] args );
		void Add( Command command );
	}
}
