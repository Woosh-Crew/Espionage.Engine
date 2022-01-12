using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Espionage.Engine.Internal.Editor
{
	[Library( "esp_editor.blueprint_window", Title = "Blueprint Editor" )]
	public class BlueprintGraphWindow : EditorWindow, ILibrary, ICallbacks
	{
		public Library ClassInfo { get; set; }

		[MenuItem( "Tools/Blueprint Editor" )]
		private static void ShowEditor()
		{
			var lib = Library.Database.Get<BlueprintGraphWindow>();
			var wind = EditorWindow.CreateWindow<BlueprintGraphWindow>();
			wind.titleContent = new GUIContent( lib.Title, lib.Description );
		}

		private void OnEnable()
		{
			ClassInfo = Library.Database.Get( GetType() );
		}

		private void CreateGUI()
		{
			// Add Stylesheet
			var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>( "Assets/Espionage.Engine/Editor/Styles/Blueprints/BlueprintGraphWindow.uss" );
			rootVisualElement.styleSheets.Add( styleSheet );

			CreateToolbar();
		}

		private void CreateToolbar()
		{
			rootVisualElement.Add<VisualElement>( "Toolbar" );
			rootVisualElement.Add<VisualElement>( "Window" );
		}
	}
}

