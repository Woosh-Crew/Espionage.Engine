using System.Threading.Tasks;
using UnityEngine;

namespace Espionage.Engine.Internal
{
	public class RuntimeConsoleProvider : IConsoleProvider
	{
		public RuntimeConsoleProvider( ICommandProvider provider )
		{
			CommandProvider = provider;
		}

		public ICommandProvider CommandProvider { get; private set; }
		public ILoggingProvider LoggingProvider { get; private set; }

		public Task Initialize()
		{
			return Task.Run( async () =>
			{
				if ( CommandProvider is null )
				{
					Debug.LogError( "Command Provider was NULL" );
					return;
				}

				await CommandProvider?.Initialize();

				Console.SetupDefaults();

				// Run all launch args
				foreach ( var item in System.Environment.GetCommandLineArgs() )
					CommandProvider?.LaunchArgs( item );
			} );
		}
	}
}
