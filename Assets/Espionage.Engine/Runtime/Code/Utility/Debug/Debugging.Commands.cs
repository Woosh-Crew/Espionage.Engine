namespace Espionage.Engine
{
	public static partial class Debugging
	{
		[Debugging.Cmd( "clear", "cls" )]
		private static void ClearCmd()
		{
			Log.Clear();
		}

		[Debugging.Cmd( "help", "?" )]
		private static void HelpCmd()
		{
			Log.Info( "All Commands" );

			foreach ( var item in Console.All )
			{
				Log.Info( $"{item.Name} - {item.Help}" );
			}
		}

		[Debugging.Cmd( "quit", "exit" )]
		private static void QuitCmd()
		{
			Log.Error( "This should probably quit the game" );
		}
	}
}
