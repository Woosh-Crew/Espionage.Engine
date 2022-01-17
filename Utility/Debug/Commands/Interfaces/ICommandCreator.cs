using System.Reflection;

namespace Espionage.Engine.Internal.Commands
{
	public interface ICommandCreator
	{
		Command[] Create( MemberInfo info );
	}
}
