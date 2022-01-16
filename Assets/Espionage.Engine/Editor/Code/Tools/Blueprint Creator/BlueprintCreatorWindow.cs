using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Espionage.Engine.Editor.Internal
{
	[Library( "esp_editor.blueprint_creator", Title = "Blueprint Creator", Help = "Create blueprints using a handy window", Group = "Blueprints" )]
	[StyleSheet( "Assets/Espionage.Engine/Editor/Styles/Blueprints/BlueprintCreatorWindow.uss" )]
	[Icon( EditorIcons.Construct )]
	public class BlueprintCreatorWindow : Tool
	{
		[MenuItem( "Tools/Blueprint/Creator", false, 0 )]
		private static void ShowEditor()
		{
			var wind = GetWindow<BlueprintCreatorWindow>();
		}
	}
}
