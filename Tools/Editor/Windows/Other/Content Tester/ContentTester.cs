using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Espionage.Engine.Editor;
using Espionage.Engine.Resources;

namespace Espionage.Engine.Tools.Editor
{
	[Title( "Content Tester" ), Group( "Tool" ), Icon( EditorIcons.Game ), HelpURL( "https://github.com/Woosh-Crew/Espionage.Engine/wiki" )]
	public class ContentTester : EditorTool
	{
		[MenuItem( "Tools/Content Tester _F7", false, -150 )]
		private static void ShowEditor()
		{
			var wind = GetWindow<ContentTester>();
		}

		protected override void OnCreateGUI()
		{
			var texture = ClassInfo.Components.Get<IconAttribute>().Icon;
			var icon = new Image() { image = texture };

			var header = new HeaderBar( ClassInfo.Title, ClassInfo.Help, icon, "Header-Bottom-Border" );
			rootVisualElement.Add( header );

			// Map testing

			var button = new Button( () =>
			{
				var path = EditorUtility.OpenFilePanel( "Load .map File", "Exports/Maps", "map" );
				Map.Find( path, () => new Map( path ) ).Load();
			} ) { text = "Test Map" };

			rootVisualElement.Add( button );
		}
	}
}
