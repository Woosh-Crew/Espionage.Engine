using System;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.Toolbars;
using UnityEngine;
using Espionage.Engine.Editor;

namespace Espionage.Engine.Tools.Editor
{
	/// <summary> A Tool is just an EditorWindow with 
	/// ILibrary and callbacks registered and a menu bar </summary>
	public class EditorTool : EditorWindow, ILibrary, ICallbacks
	{
		public Library ClassInfo { get; private set; }

		private void Awake()
		{
			Callback.Register( this );
		}

		protected virtual void OnEnable()
		{
			ClassInfo = Library.Database[GetType()];

			titleContent = new GUIContent( ClassInfo.Title, ClassInfo.Help );

			if ( ClassInfo.Components.TryGet<IconAttribute>( out var icon ) )
			{
				titleContent.image = icon.Icon;
			}
		}

		private void OnDestroy()
		{
			Callback.Unregister( this );
		}

		private void CreateGUI()
		{
			if ( ClassInfo.Components.TryGet<StyleSheetAttribute>( out var style ) )
			{
				rootVisualElement.styleSheets.Add( style.Style );
			}

			if ( MenuBarPosition == MenuBar.Position.Top )
			{
				CreateMenuBar( MenuBarPosition );
			}

			OnCreateGUI();

			if ( MenuBarPosition == MenuBar.Position.Bottom )
			{
				CreateMenuBar( MenuBarPosition );
			}
		}

		protected virtual void OnCreateGUI() { }

		//
		// Menu Bar
		//

		protected virtual MenuBar.Position MenuBarPosition => 0;

		private MenuBar _menuBar;

		private void CreateMenuBar( MenuBar.Position pos )
		{
			_menuBar = new MenuBar( pos );
			rootVisualElement.Add( _menuBar );

			OnMenuBarCreated( _menuBar );

			// Create Tools Menu
			var toolsMenu = new GenericMenu();
			_menuBar.Add( "Tools", toolsMenu );

			foreach ( var item in Library.Database.GetAll<EditorTool>() )
			{
				if ( !string.Equals( item.Group, "hidden", StringComparison.CurrentCultureIgnoreCase ) )
				{
					toolsMenu.AddItem( new GUIContent( string.IsNullOrEmpty( item.Group ) ? "" : $"{item.Group}/" + item.Title ), false, () => GetWindow( item.Class ) );
				}
			}

			// Create Help Menu
			var helpMenu = new GenericMenu();

			// About
			helpMenu.AddItem( new GUIContent( "About" ), false, AboutWindow.ShowWindow );

			// Wiki
			var helpUrl = ClassInfo.Components.Get<HelpURLAttribute>()?.URL ?? "https://github.com/Woosh-Crew/Espionage.Engine/wiki";
			helpMenu.AddItem( new GUIContent( "Wiki" ), false, () => Application.OpenURL( helpUrl ) );
			_menuBar.Add( "Help", helpMenu );
		}

		protected virtual void OnMenuBarCreated( MenuBar bar ) { }

		//
		// Dock Bar Button
		//

		private void ShowButton( Rect rect )
		{
			using ( _ = new GUILayout.AreaScope( rect ) )
			{
				OnBarButton();
			}
		}

		/// <summary> IMGUI Container for Dock Bar Button </summary>
		protected virtual void OnBarButton() { }
	}
}
