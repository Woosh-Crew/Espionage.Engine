using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Espionage.Engine.Editor;
using Espionage.Engine.Resources;

namespace Espionage.Engine.Tools.Editor
{
	[Title( "Content Tester" ), Help( "Test Assets such as Maps and Models" ), Icon( EditorIcons.Game )]
	public class ContentTester : EditorTool
	{
		[MenuItem( "Tools/Espionage.Engine/Content Tester _F7", priority = -25 )]
		private static void ShowEditor()
		{
			GetWindow<ContentTester>();
		}

		protected override void OnCreateGUI()
		{
			var header = new HeaderBar( ClassInfo.Title, ClassInfo.Help, null, "Header-Bottom-Border" );
			rootVisualElement.Add( header );

			// Map testing

			var button = new Button( () =>
			{
				// Get all Map Types

				var providers = Library.Database.GetAll<IMapProvider>();
				var extensions = providers.Where( e => e.Components.Get<FileAttribute>() != null );

				var path = EditorUtility.OpenFilePanel( "Load .map File", "Exports/Maps", string.Join( ',', extensions.Select( e => e.Components.Get<FileAttribute>().Extension ) ) );

				if ( string.IsNullOrEmpty( path ) )
				{
					Debugging.Log.Info( "No Map Selected" );
					return;
				}

				Map.Find( path ).Load();
			} ) { text = "Test Map" };

			rootVisualElement.Add( button );
		}
	}
}
