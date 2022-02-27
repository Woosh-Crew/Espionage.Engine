using UnityEngine.UIElements;
using UnityEditor;
using Espionage.Engine.Editor;


namespace Espionage.Engine.Tools.Editor
{
	[Title( "Library Database" ), Group( "Debug" ), Icon( EditorIcons.Terminal )]
	public class LibraryDatabaseViewer : EditorTool
	{
		[MenuItem( "Tools/Espionage.Engine/Debug/Library Viewer", false, 500 )]
		private static void ShowEditor()
		{
			GetWindow<LibraryDatabaseViewer>();
		}

		protected override void OnCreateGUI()
		{
			foreach ( var item in Library.Database.All )
			{
				rootVisualElement.Add( new Label( $"{item.Name} - {item.Title}" ) );
			}
		}
	}
}
