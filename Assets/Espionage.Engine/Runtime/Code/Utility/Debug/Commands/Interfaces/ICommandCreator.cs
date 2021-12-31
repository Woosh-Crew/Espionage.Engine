using System.Reflection;

namespace Espionage.Engine.Internal
{
	public interface ICommandCreator
	{
		Command[] Create( MemberInfo info );
	}
}
