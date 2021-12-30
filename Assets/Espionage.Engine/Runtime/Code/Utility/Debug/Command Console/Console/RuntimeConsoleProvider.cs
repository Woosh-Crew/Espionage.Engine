using System.Threading.Tasks;
using UnityEngine;

namespace Espionage.Engine.Internal
{
	class RuntimeConsoleProvider : IConsoleProvider
	{
		public RuntimeConsoleProvider( ICommandProvider provider )
		{
			CommandProvider = provider;
		}

		public ICommandProvider CommandProvider { get; set; }

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

				// Initialize default commands from scratch, that way they are present
				// on every ICommandProvider.
				var quitCmd = new Console.Command() { Name = "quit", Help = "Quits the game" };
				quitCmd.WithAction( ( e ) => Console.QuitCmd() );
				CommandProvider?.Add( quitCmd );

				var clearCmd = new Console.Command() { Name = "clear", Help = "Clears all logs" };
				clearCmd.WithAction( ( e ) => Console.ClearCmd() );
				CommandProvider?.Add( clearCmd );

				var helpCmd = new Console.Command() { Name = "help", Help = "Dumps all commands, or anything starting with input" };
				helpCmd.WithAction( ( e ) => Console.HelpCmd() );
				CommandProvider?.Add( helpCmd );

				Debug.Log( $"Found {CommandProvider.All.Count} Commands" );

				// Run all launch args
				foreach ( var item in System.Environment.GetCommandLineArgs() )
					CommandProvider?.LaunchArgs( item );
			} );
		}
	}
}
