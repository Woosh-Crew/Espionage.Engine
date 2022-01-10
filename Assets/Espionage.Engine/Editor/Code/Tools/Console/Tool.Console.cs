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
		[MenuItem( "Tools/Terminal" )]
		public static void CreateWindow()
		{
			var info = Library.Database.Get<EditorConsoleTool>();
			var terminal = GetWindow<EditorConsoleTool>();
			terminal.titleContent = new GUIContent( info.Title, AssetDatabase.LoadAssetAtPath<Texture>( info.Icon ), info.Description );
		}

		//
		// Instance
		//

		public Library ClassInfo { get; private set; }

		private void Start()
		{
			ClassInfo = Library.Database.Get( GetType() );
		}

		//
		// GUI
		//

		private const string StylePath = "Assets/Espionage.Engine/Editor/Styles/Terminal.uss";

		private TextField _input;

		private void CreateGUI()
		{
			var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>( StylePath );

			var root = rootVisualElement;
			root.styleSheets.Add( styleSheet );

			_input = new TextField() { name = "Input" };
			root.Add( _input );

			// Register input
			_input.RegisterCallback<UnityEngine.UIElements.KeyDownEvent>( ( e ) =>
			{
				// If return, submit command
				if ( e.keyCode is KeyCode.Return )
				{
					Submit( _input.text );
					Clear();
					_input.Focus();
				}

				// Clear console
				if ( e.keyCode is KeyCode.X && e.ctrlKey )
				{
					Clear();
				}

				// If arrow keys, cycle through history & suggestions
				if ( e.keyCode is KeyCode.UpArrow )
				{
					Debug.Log( "History Up" );
				}

				if ( e.keyCode is KeyCode.DownArrow )
				{
					Debug.Log( "History Down" );
				}

				// Autofil
				if ( e.keyCode is KeyCode.RightArrow )
				{
					var autofill = Debugging.Console.Find( _input.value ).FirstOrDefault();
					if ( !string.IsNullOrEmpty( autofill ) )
					{
						_input.value = autofill;
					}
				}
			} );

			// Any input, try autofil
			_input.RegisterCallback<InputEvent>( ( e ) =>
			{
				if ( string.IsNullOrEmpty( _input.text ) )
					return;

				var commands = Debugging.Console.Find( _input.text );
			} );

		}

		public void Submit( string command )
		{
			if ( string.IsNullOrEmpty( command ) )
				return;
			Debugging.Log.Add( new Logging.Entry() { Message = command, Type = Logging.Entry.Level.Info } );
			Debugging.Console.Invoke( command );
		}

		public void Clear()
		{
			_input.value = string.Empty;
		}
	}
}
