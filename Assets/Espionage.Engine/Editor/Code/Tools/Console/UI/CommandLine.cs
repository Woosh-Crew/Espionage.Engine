using UnityEditor;
using UnityEditor.Toolbars;
using UnityEditor.Overlays;
using UnityEngine.UIElements;
using UnityEngine;

namespace Espionage.Engine.Internal
{
	[Overlay( typeof( SceneView ), "esp.cmdl", "Command Line" )]
	public class CommandLine : Overlay
	{
		private TextField Input { get; set; }

		public override VisualElement CreatePanelContent()
		{
			var root = new VisualElement();

			root.style.width = new StyleLength( 256 );

			var inputLabel = root.Add<Label>( classes: "Prefix" );
			inputLabel.text = "Input";
			Input = new TextField() { name = "Input" };
			root.Add( Input );

			// Register input
			Input.RegisterCallback<UnityEngine.UIElements.KeyDownEvent>( ( e ) =>
			{
				// If return, submit command
				if ( e.keyCode is KeyCode.Return )
				{
					Submit( Input.text );
					Clear();
					Input.Focus();
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
				if ( e.keyCode is KeyCode.RightArrow && Input.cursorIndex == Input.text.Length )
				{
					Debug.Log( "Autofil" );
				}
			} );

			// Any input, try autofil
			Input.RegisterCallback<InputEvent>( ( e ) =>
			{
				if ( string.IsNullOrEmpty( Input.text ) )
					return;

				var commands = Console.Provider.CommandProvider.Find( Input.text );
			} );

			return root;
		}

		public void Submit( string command )
		{
			if ( string.IsNullOrEmpty( command ) )
				return;

			Console.Invoke( command );
		}

		public void Clear()
		{
			Input.value = string.Empty;
		}
	}
}
