using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Espionage.Engine.Internal.Editor
{
	[Library( "esp_editor.blueprint_creator",
		Title = "Blueprint Creator",
		Help = "Create blueprints using a handy window",
	 	Icon = "Assets/Espionage.Engine/Editor/Styles/Icons/baseline_view_in_ar_white_48dp.png"
	)]
	public class BlueprintCreatorWindow : EditorWindow, ILibrary, ICallbacks
	{
		public Library ClassInfo { get; set; }

		[MenuItem( "Tools/Blueprint Creator" )]
		private static void ShowEditor()
		{
			var lib = Library.Database.Get<BlueprintCreatorWindow>();
			var wind = EditorWindow.CreateInstance<BlueprintCreatorWindow>();
			wind.titleContent = new GUIContent( lib.Title, lib.Description );
			wind.ShowModalUtility();
		}

		private void OnEnable()
		{
			ClassInfo = Library.Database.Get( GetType() );
		}

		private void CreateGUI()
		{
			// Add Stylesheet
			var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>( "Assets/Espionage.Engine/Editor/Styles/Blueprints/BlueprintCreatorWindow.uss" );
			rootVisualElement.styleSheets.Add( styleSheet );

			rootVisualElement.Add( new Label( "balls" ) );
		}

	}
}
