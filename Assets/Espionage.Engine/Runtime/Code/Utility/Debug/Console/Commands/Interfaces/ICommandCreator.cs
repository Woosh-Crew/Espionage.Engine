using System.Reflection;

namespace Espionage.Engine.Internal
{
	public interface ICommandCreator
	{
		Console.Command[] Create( MemberInfo info );
	}
}
