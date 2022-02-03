using System.Collections.Generic;
using System.Reflection;

namespace Espionage.Engine.Internal.Commands
{
	public interface ICommandCreator
	{
		List<Command> Create( MemberInfo info );
	}
}
