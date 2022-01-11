using UnityEditor;
using Espionage.Engine.Editor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

namespace Espionage.Engine.Internal.Editor
{
	[Library( "esp_editor.terminal_window",
	 	Title = "Terminal",
		Help = "Interface with the Espionage.Engine command line",
	  	Icon = "Assets/Espionage.Engine/Editor/Styles/Icons/outline_terminal_white_48dp.png" )]
	public class EditorConsoleTool : EditorWindow, ILibrary, ICallbacks
	{
		public Library ClassInfo { get; set; }

		public static EditorConsoleTool CreatePopup()
		{
			var popup = ScriptableObject.CreateInstance<EditorConsoleTool>();
			popup.position = new Rect( Event.current.mousePosition, Vector2.one * 64 );
			popup.ShowPopup();
			return popup;
		}

		private void OnEnable()
		{
			ClassInfo = Library.Database.Get( GetType() );
		}

		[EditorCallback.SceneView.Drawing]
		private static void Events( SceneView sceneView )
		{
			var e = Event.current;

			switch ( e.type )
			{
				case EventType.KeyDown:
					{
						if ( e.keyCode == KeyCode.F1 )
						{
							ToggleWindow();
						}

						break;
					}
			}
		}

		private static EditorConsoleTool _window;
		private static bool _state;
		private static void ToggleWindow()
		{
			_state = !_state;

			if ( _state )
				_window.Close();
			else
				_window = CreatePopup();
		}


		public static void Submit( string command )
		{
			if ( string.IsNullOrEmpty( command ) )
				return;

			Debugging.Log.Add( new Logging.Entry() { Message = command, Type = Logging.Entry.Level.Info } );
			Debugging.Console.Invoke( command );
		}

		public static void Clear()
		{
		}
	}
}
