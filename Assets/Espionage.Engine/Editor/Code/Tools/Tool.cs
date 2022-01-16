using UnityEditor;
using UnityEngine;

namespace Espionage.Engine.Editor.Internal
{
	/// <summary> A Tool is just an EditorWindow with 
	/// ILibrary and callbacks registered and a menu bar </summary>
	public class Tool : EditorWindow, ILibrary, ICallbacks
	{
		public Library ClassInfo { get; private set; }
		
		private void Awake()
		{
			Callback.Register( this );
		}

		protected virtual void OnEnable()
		{
			ClassInfo = Library.Database.Get( GetType() );
			titleContent = new GUIContent( ClassInfo.title, ClassInfo.help );

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
				rootVisualElement.styleSheets.Add( style.Style );

			if ( MenuBarPosition == 0 )
				CreateMenuBar( MenuBarPosition );

			OnCreateGUI();

			if ( MenuBarPosition == 1 )
				CreateMenuBar( MenuBarPosition );
		}

		protected virtual void OnCreateGUI() { }

		//
		// Menu Bar
		//

		/// <summary> 0 = Top, 1 = Bottom </summary>
		protected virtual int MenuBarPosition => 0;

		private MenuBar _menuBar;

		private void CreateMenuBar( int pos = 1 )
		{
			_menuBar = new MenuBar( pos );
			rootVisualElement.Add( _menuBar );

			OnMenuBarCreated( _menuBar );

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
	}
}
