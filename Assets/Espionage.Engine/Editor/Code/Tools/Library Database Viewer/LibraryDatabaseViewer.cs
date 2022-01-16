using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace Espionage.Engine.Editor.Internal
{
	[Library( Title = "Library Viewer" )]
	[Icon( EditorIcons.Terminal )]
	public class LibraryDatabaseViewer : Tool
	{
		[MenuItem( "Tools/Debug/Library Viewer", false, 500 )]
		private static void ShowEditor()
		{
			var wind = EditorWindow.GetWindow<LibraryDatabaseViewer>();
		}

		protected override void OnCreateGUI()
		{
			base.OnCreateGUI();

			foreach ( var item in Library.Database.All )
			{
				rootVisualElement.Add( new Label( $"{item.name} - {item.title}" ) );
			}
		}
	}
}
