using UnityEngine.UIElements;
using UnityEditor;
using Espionage.Engine.Editor;


namespace Espionage.Engine.Tools.Editor
{
	[Library( Title = "Library Viewer", Group = "Debug" ), Icon( EditorIcons.Terminal )]
	public class LibraryDatabaseViewer : Tool
	{
		[MenuItem( "Tools/Debug/Library Viewer", false, 500 )]
		private static void ShowEditor()
		{
			var wind = GetWindow<LibraryDatabaseViewer>();
		}

		protected override void OnCreateGUI()
		{
			base.OnCreateGUI();

			foreach ( var item in Library.Database.All )
			{
				rootVisualElement.Add( new Label( $"{item.Name} - {item.Title}" ) );
			}
		}
	}
}
