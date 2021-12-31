using System.Threading.Tasks;

namespace Espionage.Engine.Internal
{
	public interface IConsoleProvider
	{
		Task Initialize() { return null; }

		ICommandProvider CommandProvider { get; }
		ILoggingProvider LoggingProvider { get; }
	}
}
