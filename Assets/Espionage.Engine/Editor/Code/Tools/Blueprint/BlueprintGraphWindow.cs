using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Espionage.Engine.Internal.Editor
{
	[Library( "esp_editor.blueprint_window",
		Title = "Blueprint Editor",
		Help = "Interface with a blueprints node tree",
	  	Icon = "Assets/Espionage.Engine/Editor/Styles/Icons/baseline_view_in_ar_white_48dp.png"
	)]
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

			CreateMenuBar();

			var window = new VisualElement() { name = "Window" };
			rootVisualElement.Add( window );

			var blueprintWatermark = new VisualElement() { name = "Blueprint-Watermark" };
			window.Add( blueprintWatermark );
			var iconTexture = AssetDatabase.LoadAssetAtPath<Texture>( ClassInfo.Icon );

			blueprintWatermark.Add( new Image() { image = iconTexture } );
			blueprintWatermark.Add( new Label( "BLUEPRINT" ) );
		}

		private VisualElement _toolbar;

		private void CreateMenuBar()
		{
			Button CreateDropdown( string text )
			{
				var button = new Button() { text = text };
				button.AddToClassList( "MenuBar-Button" );
				return button;
			}

			_toolbar = rootVisualElement.Add<VisualElement>( "MenuBar" );

			// Buttons
			_toolbar.Add( CreateDropdown( $"File" ) );
			_toolbar.Add( CreateDropdown( $"Edit" ) );
			_toolbar.Add( CreateDropdown( $"Asset" ) );
			_toolbar.Add( CreateDropdown( $"View" ) );
			_toolbar.Add( CreateDropdown( $"Options" ) );
			_toolbar.Add( CreateDropdown( $"Help" ) );
		}
	}
}

