using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using Espionage.Engine.Editor;
using Espionage.Engine.Entities;

namespace Espionage.Engine.Internal.Editor
{
	[Library( "esp_editor.blueprint_window", Title = "Blueprint Editor", Help = "Interface with a blueprints node tree" )]
	[Icon( EditorIcons.Blueprint )]
	public class BlueprintGraphWindow : EditorWindow, ILibrary, ICallbacks
	{
		public Library ClassInfo { get; set; }

		[MenuItem( "Tools/Blueprint Editor" )]
		private static void ShowEditor()
		{
			var lib = Library.Database.Get<BlueprintGraphWindow>();
			var wind = EditorWindow.GetWindow<BlueprintGraphWindow>();
			wind.titleContent = new GUIContent( lib.Title, lib.Help );
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

			// Split View
			var panel = new TwoPaneSplitView( 0, 300, TwoPaneSplitViewOrientation.Horizontal );
			panel.Add( CreateInfoBoard() );
			panel.Add( CreateGraphView() );

			rootVisualElement.Add( panel );

			CreateMenuBar();

			Selection.selectionChanged -= OnSelectionChange;
			Selection.selectionChanged += OnSelectionChange;

			OnSelectionChange();
		}

		private void OnSelectionChange()
		{
			if ( Selection.activeObject is NodeTree tree )
			{
				_graph.LoadGraph( tree );
			}
		}

		//
		// Inspector
		//

		private VisualElement CreateInfoBoard()
		{
			var root = new VisualElement() { name = "InfoBoard" };

			// Split View
			var panel = new TwoPaneSplitView( 0, 256, TwoPaneSplitViewOrientation.Vertical );

			panel.Add( CreateInspector() );
			panel.Add( CreateBlackboard() );

			root.Add( panel );

			return root;
		}

		private VisualElement CreateBlackboard()
		{
			var root = new VisualElement() { name = "Blackboard" };
			root.Add( CreateTitlebar( "Blackboard", AssetDatabase.LoadAssetAtPath<Texture>( EditorIcons.Dashboard ) ) );

			return root;

		}

		private VisualElement CreateInspector()
		{
			var root = new VisualElement() { name = "Inspector" };
			root.Add( CreateTitlebar( "Inspector", AssetDatabase.LoadAssetAtPath<Texture>( EditorIcons.Info ) ) );

			return root;
		}

		private VisualElement CreateTitlebar( string title, Texture icon )
		{
			var root = new VisualElement() { name = "TitleBar" };
			root.Add( new Image() { image = icon } );
			root.Add( new Label( title ) );
			return root;
		}

		//
		// Graph View
		//

		private BlueprintGraphView _graph;
		private VisualElement _infoBar;

		private VisualElement CreateGraphView()
		{
			Button CreateButton( string text, Texture icon, out Image image, Action onClick = null )
			{
				var button = new Button( onClick ) { tooltip = text };
				button.AddToClassList( "Info-Bar-Button" );
				image = new Image() { image = icon };
				button.Add( image );

				return button;
			}

			var root = new VisualElement();

			root.Add( CreateTitlebar( "Node Graph", AssetDatabase.LoadAssetAtPath<Texture>( EditorIcons.NodeTree ) ) );

			_graph = new BlueprintGraphView()
			{
				name = "Window"
			};

			root.Add( _graph );

			// Watermark

			var blueprintWatermark = new VisualElement() { name = "Blueprint-Watermark" };
			_graph.Add( blueprintWatermark );

			var iconTexture = ClassInfo.Components.Get<IconAttribute>().Icon;
			blueprintWatermark.Add( new Image() { image = iconTexture } );
			blueprintWatermark.Add( new Label( "BLUEPRINT" ) );

			// Info Bar

			_infoBar = new VisualElement() { name = "Info-Bar" };
			_graph.Add( _infoBar );
			{
				// Left Side

				var left = new VisualElement();
				left.AddToClassList( "Left" );
				_infoBar.Add( left );

				left.Add( new Label() { text = "Door Blueprint" } );

				// Right Side

				var right = new VisualElement();
				right.AddToClassList( "Right" );
				_infoBar.Add( right );

				var saveIcon = AssetDatabase.LoadAssetAtPath<Texture>( EditorIcons.Save );
				right.Add( CreateButton( "Save", saveIcon, out _ ) );

				var zoomOutIcon = AssetDatabase.LoadAssetAtPath<Texture>( EditorIcons.ZoomOut );
				var zoomInIcon = AssetDatabase.LoadAssetAtPath<Texture>( EditorIcons.ZoomIn );

				var maximiseButton = CreateButton( "", maximized ? zoomInIcon : zoomOutIcon, out var maximiseImage );
				maximiseButton.tooltip = maximized ? "Zoom In" : "Zoom Out";

				maximiseButton.clicked += () =>
				{
					maximized = !maximized;
					maximiseImage.image = maximized ? zoomInIcon : zoomOutIcon;
					maximiseButton.tooltip = maximized ? "Zoom In" : "Zoom Out";
				};


				right.Add( maximiseButton );

				var infoIcon = AssetDatabase.LoadAssetAtPath<Texture>( EditorIcons.Info );
				right.Add( CreateButton( "Info", infoIcon, out _ ) );
			}

			return root;
		}

		//
		// Menu Bar
		//

		private VisualElement _menubar;

		private void CreateMenuBar()
		{
			Button CreateDropdown( string text, Action action = null )
			{
				var button = new Button( action ) { text = text };
				button.AddToClassList( "MenuBar-Button" );
				return button;
			}

			_menubar = rootVisualElement.Add<VisualElement>( "MenuBar" );

			// Buttons
			_menubar.Add( CreateDropdown( $"File", CreateGenericMenu ) );
			_menubar.Add( CreateDropdown( $"Edit" ) );
			_menubar.Add( CreateDropdown( $"Nodes" ) );
			_menubar.Add( CreateDropdown( $"View" ) );
			_menubar.Add( CreateDropdown( $"Options" ) );
			_menubar.Add( CreateDropdown( $"Help" ) );
		}

		private void CreateGenericMenu()
		{
			// create the menu and add items to it
			GenericMenu menu = new GenericMenu();

			menu.AddItem( new GUIContent( "Save" ), false, null );
			menu.AddItem( new GUIContent( "Save As" ), false, null );
			menu.AddSeparator( "" );
			menu.AddItem( new GUIContent( "Open" ), false, null );
			menu.AddItem( new GUIContent( "Recent Files/To bad!" ), false, null );

			// display the menu
			menu.ShowAsContext();
		}
	}
}

