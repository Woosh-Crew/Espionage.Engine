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
				var quitCmd = new Console.Command() { Name = "quit", Help = "Quits the game", Owner = typeof( Console ) };
				quitCmd.WithAction( ( e ) => QuitCmd() );
				CommandProvider?.Add( quitCmd );

				var helpCmd = new Console.Command() { Name = "help", Help = "Dumps all commands, or anything starting with input", Owner = typeof( Console ) };
				helpCmd.WithAction( ( e ) => HelpCmd() );
				CommandProvider?.Add( helpCmd );

				Debug.Log( $"Found {CommandProvider.All.Count} Commands" );

				// Run all launch args
				foreach ( var item in System.Environment.GetCommandLineArgs() )
					CommandProvider?.LaunchArgs( item );
			} );
		}

		//
		// Commands
		//

		public static void HelpCmd()
		{
			Debug.Log( $"Commands" );

			foreach ( var item in Console.Provider.CommandProvider?.All )
				Debug.Log( $"{item.Name} - {item.Owner.FullName}" );
		}

		public static void QuitCmd()
		{
			Application.Quit();
		}
	}
}
