using System.Linq;
using UnityEngine;

namespace Espionage.Engine
{
	public static partial class Debugging
	{
		[Cmd( "clear", "cls", Help = "Clears everything in the log" )]
		private static void ClearCmd()
		{
			Log.Clear();
		}

		[Cmd( "help", "?", Help = "Dumps all commands and their help message" )]
		private static void HelpCmd( string prefix = null )
		{
			// If we have no prefix
			if ( string.IsNullOrEmpty( prefix ) )
			{
				Log.Info( "All Commands" );

				foreach ( var item in Console.All )
				{
					Log.Info( $"[ {item.Name} ] - {item.Help}" );
				}

				return;
			}

			Log.Info( $"{prefix} Commands" );

			foreach ( var item in Console.All.Where( e => e.Name.StartsWith( prefix ) ) )
			{
				Log.Info( $"{item.Name} - {item.Help}" );
			}
		}

		[Cmd( "quit", "exit", Help = "Quits the application" )]
		private static void QuitCmd()
		{
			Log.Warning( "This should probably quit the game" );

#if UNITY_STANDALONE
			Application.Quit();
#elif UNITY_EDITOR
			UnityEditor.EditorApplication.ExitPlaymode();
#endif
		}
	}
}
