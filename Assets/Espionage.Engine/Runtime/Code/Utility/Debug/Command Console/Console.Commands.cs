using UnityEngine;

namespace Espionage.Engine
{
	public static partial class Console
	{
		public static void HelpCmd()
		{
			foreach ( var item in Provider?.CommandProvider?.All )
			{
				AddLog( new Entry( $"{item.Name}", "", LogType.Log ) );
			}
			AddLog( new Entry( "Commands", "", LogType.Log ) );
		}

		public static void ClearCmd()
		{
			_logs.Clear();
			OnClear?.Invoke();
		}

		public static void QuitCmd()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.ExitPlaymode();
#else
			Application.Quit();
#endif
		}
	}
}
