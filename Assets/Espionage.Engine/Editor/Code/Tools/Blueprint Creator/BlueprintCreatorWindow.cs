using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Espionage.Engine.Editor.Internal
{
	[Library( "esp_editor.blueprint_creator", Title = "Blueprint Creator", Help = "Create blueprints using a handy window" )]
	[Icon( EditorIcons.Construct ), StyleSheet( "Assets/Espionage.Engine/Editor/Styles/Blueprints/BlueprintCreatorWindow.uss" )]
	public class BlueprintCreatorWindow : Tool
	{
		[MenuItem( "Tools/Blueprint/Creator" )]
		private static void ShowEditor()
		{
			var wind = EditorWindow.GetWindow<BlueprintCreatorWindow>();
		}
	}
}
