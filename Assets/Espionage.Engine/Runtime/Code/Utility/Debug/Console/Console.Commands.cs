using UnityEngine;

namespace Espionage.Engine
{
	public static partial class Console
	{
		[Console.Cmd( "help", Layer = Layer.Runtime | Layer.Editor )]
		private static void HelpCmd()
		{
			foreach ( var item in _commands.Values )
			{
				if ( !Application.isEditor && item.Layer is Layer.Editor )
					continue;

				AddLog( new Entry( $"{item.Name}", "", Layer.Editor, LogType.Log ) );
			}
			AddLog( new Entry( "Commands", "", Layer.Runtime, LogType.Log ) );
		}

		[Console.Cmd( "clear", Layer = Layer.Runtime | Layer.Editor )]
		private static void ClearCmd()
		{
			_logs.Clear();
			OnClear?.Invoke();
		}

		[Console.Cmd( "quit" )]
		private static void QuitCmd()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.ExitPlaymode();
#else
			Application.Quit();
#endif
		}
	}
}