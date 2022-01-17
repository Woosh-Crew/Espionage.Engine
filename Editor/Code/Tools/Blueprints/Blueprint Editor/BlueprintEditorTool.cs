using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using Espionage.Engine.Editor;
using Espionage.Engine.Entities;

namespace Espionage.Engine.Editor.Internal.Blueprints
{
	[Title( "Blueprint Editor" )]
	[Group( "Blueprints" )]
	[Icon( EditorIcons.Blueprint )]
	[StyleSheet( "Assets/Espionage.Engine/Editor/Styles/Blueprints/BlueprintGraphWindow.uss" )]
	public sealed class BlueprintEditorTool : Tool
	{
		[MenuItem( "Tools/Blueprint/Editor #F6", false )]
		private static void ShowEditor()
		{
			var wind = GetWindow<BlueprintEditorTool>();
		}

		protected override void OnCreateGUI()
		{
			// Split View
			var panel = new TwoPaneSplitView( 0, 300, TwoPaneSplitViewOrientation.Horizontal );
			panel.Add( CreateInfoBoard() );
			panel.Add( CreateViewer() );

			rootVisualElement.Add( panel );

			OnSelectionChange();
		}

		private void OnSelectionChange()
		{
			if ( Selection.activeObject is Blueprint blueprint )
			{
				Blueprint = blueprint;
			}
		}

		//
		// Selection
		//

		// Blueprint

		private Blueprint _blueprint;

		public Blueprint Blueprint
		{
			get => _blueprint;
			set
			{
				OnBlueprintChange( _blueprint, value );
				_blueprint = value;
			}
		}

		private void OnBlueprintChange( Blueprint oldBp, Blueprint newBp )
		{
			// Set Title Bar
			_inheritingLabel.text = newBp.ClassInfo.Title ?? "Unknown";
		}

		//
		// Menu Bar
		//

		protected override MenuBar.Position MenuBarPosition => MenuBar.Position.Bottom;

		protected override void OnMenuBarCreated( MenuBar bar )
		{
			bar.Add( "File" );
			bar.Add( "Edit" );
			bar.Add( "Nodes" );
			bar.Add( "View" );
			bar.Add( "Options" );
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

			var icon = AssetDatabase.LoadAssetAtPath<Texture>( EditorIcons.Question );
			root.Add( new HeaderBar( "Inspector", "Select an object to inspect it", new Image() { image = icon }, "Header-Bottom-Border" ) );

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

		private VisualElement _infoBar;
		private Label _inheritingLabel;

		private VisualElement _viewContainer;

		private VisualElement CreateGraphCreation()
		{
			var root = new VisualElement() { name = "GraphCreator" };

			root.Add( new Button() );

			return root;
		}

		private VisualElement CreateViewer()
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

			_viewContainer = new BlueprintGraphView( this ) { name = "Window" };

			root.Add( _viewContainer );

			// Watermark

			var blueprintWatermark = new VisualElement() { name = "Blueprint-Watermark" };
			_viewContainer.Add( blueprintWatermark );

			var iconTexture = ClassInfo.Components.Get<IconAttribute>().Icon;
			blueprintWatermark.Add( new Image() { image = iconTexture } );
			blueprintWatermark.Add( new Label( "BLUEPRINT" ) );

			// Info Bar

			_infoBar = new VisualElement() { name = "Info-Bar" };
			_viewContainer.Add( _infoBar );
			{
				// Left Side

				var left = new VisualElement();
				left.AddToClassList( "Left" );
				_infoBar.Add( left );

				_inheritingLabel = new Label() { text = "Unknown" };
				left.Add( _inheritingLabel );

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
				right.Add( CreateButton( "Info", infoIcon, out _, null ) );
			}

			return root;
		}
	}
}
