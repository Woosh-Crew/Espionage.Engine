using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;

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

			CreateGraphView();
			CreateMenuBar();
		}

		//
		// Graph View
		//

		private BlueprintGraphView _graphView;
		private VisualElement _infoBar;

		private void CreateGraphView()
		{
			Button CreateButton( string text, Texture icon, out Image image, Action onClick = null )
			{
				var button = new Button( onClick ) { tooltip = text };
				button.AddToClassList( "Info-Bar-Button" );
				image = new Image() { image = icon };
				button.Add( image );

				return button;
			}


			_graphView = new BlueprintGraphView()
			{
				name = "Window"
			};

			rootVisualElement.Add( _graphView );

			// Vignette

			// var image = new VisualElement() { name = "Vignette" };
			// _graphView.Add( image );

			// Watermark

			var blueprintWatermark = new VisualElement() { name = "Blueprint-Watermark" };
			_graphView.Add( blueprintWatermark );

			var iconTexture = AssetDatabase.LoadAssetAtPath<Texture>( ClassInfo.Icon );
			blueprintWatermark.Add( new Image() { image = iconTexture } );
			blueprintWatermark.Add( new Label( "BLUEPRINT" ) );

			// Info Bar

			_infoBar = new VisualElement() { name = "Info-Bar" };
			_graphView.Add( _infoBar );
			{
				// Left Side

				var left = new VisualElement();
				left.AddToClassList( "Left" );
				_infoBar.Add( left );

				left.Add( new Label() { text = "Weapon Blueprint" } );

				// Right Side

				var right = new VisualElement();
				right.AddToClassList( "Right" );
				_infoBar.Add( right );

				var saveIcon = AssetDatabase.LoadAssetAtPath<Texture>( "Assets/Espionage.Engine/Editor/Styles/Icons/baseline_save_white_48dp.png" );
				right.Add( CreateButton( "Save", saveIcon, out _ ) );

				var zoomOutIcon = AssetDatabase.LoadAssetAtPath<Texture>( "Assets/Espionage.Engine/Editor/Styles/Icons/baseline_zoom_out_map_white_48dp.png" );
				var zoomInIcon = AssetDatabase.LoadAssetAtPath<Texture>( "Assets/Espionage.Engine/Editor/Styles/Icons/baseline_zoom_in_map_white_48dp.png" );

				var maximiseButton = CreateButton( "", maximized ? zoomInIcon : zoomOutIcon, out var maximiseImage );
				maximiseButton.tooltip = maximized ? "Zoom In" : "Zoom Out";

				maximiseButton.clicked += () =>
				{
					maximized = !maximized;
					maximiseImage.image = maximized ? zoomInIcon : zoomOutIcon;
					maximiseButton.tooltip = maximized ? "Zoom In" : "Zoom Out";
				};


				right.Add( maximiseButton );

				var infoIcon = AssetDatabase.LoadAssetAtPath<Texture>( "Assets/Espionage.Engine/Editor/Styles/Icons/baseline_info_white_48dp.png" );
				right.Add( CreateButton( "Info", infoIcon, out _ ) );
			}
		}

		//
		// Menu Bar
		//

		private VisualElement _menubar;

		private void CreateMenuBar()
		{
			Button CreateDropdown( string text )
			{
				var button = new Button() { text = text };
				button.AddToClassList( "MenuBar-Button" );
				return button;
			}

			_menubar = rootVisualElement.Add<VisualElement>( "MenuBar" );

			// Buttons
			_menubar.Add( CreateDropdown( $"File" ) );
			_menubar.Add( CreateDropdown( $"Edit" ) );
			_menubar.Add( CreateDropdown( $"Asset" ) );
			_menubar.Add( CreateDropdown( $"View" ) );
			_menubar.Add( CreateDropdown( $"Options" ) );
			_menubar.Add( CreateDropdown( $"Help" ) );
		}
	}
}

