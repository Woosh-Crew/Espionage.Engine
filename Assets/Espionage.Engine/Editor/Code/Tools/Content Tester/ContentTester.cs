using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Espionage.Engine.Editor.Internal
{
	[Library( "tool.content_tester", Title = "Content Tester", Help = "Quickly test content in game" )]
	[Icon( EditorIcons.Game ), HelpURL( "https://github.com/Woosh-Crew/Espionage.Engine/wiki" )]
	public class ContentTester : Tool
	{
		[MenuItem( "Tools/Content Tester _F7", false, -150 )]
		private static void ShowEditor()
		{
			var wind = EditorWindow.GetWindow<ContentTester>();
		}

		protected override void OnCreateGUI()
		{
			var texture = ClassInfo.Components.Get<IconAttribute>().Icon;
			var icon = new Image() { image = texture };

			var header = new HeaderBar( ClassInfo.Title, ClassInfo.Help, icon, "Header-Bottom-Border" );
			rootVisualElement.Add( header );
		}
	}
}
