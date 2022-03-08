using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Espionage.Engine.Editor;

namespace Espionage.Engine.Tools.Editor
{
	[Group( "Debug" ), Title( "GUID Grabber" )]
	internal class GuidGrabber : EditorTool
	{
		private Label _label;
		private Button _button;

		protected override void OnCreateGUI()
		{
			_button = new Button { text = "Copy GUID to Clipboard" };
			_button.clicked += () => CopyToClipboard( AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath( Selection.activeObject ) ) );
			_label = new Label();

			rootVisualElement.Add( _label );
			rootVisualElement.Add( _button );
		}

		private void OnSelectionChange()
		{
			var guid = AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath( Selection.activeObject ) );
			_label.text = $"{Selection.activeObject.name} - {guid}";
		}

		private void CopyToClipboard( string thing )
		{
			GUIUtility.systemCopyBuffer = thing;
		}
	}
}
