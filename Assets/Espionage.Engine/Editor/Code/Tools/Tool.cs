
using Espionage.Engine.Editor.Internal;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Espionage.Engine.Editor.Internal
{
	/// <summary> A Tool is just an EditorWindow with 
	/// ILibrary and callbacks registered and a menu bar</summary>
	public class Tool : EditorWindow, ILibrary, ICallbacks
	{
		public Library ClassInfo { get; set; }

		private void Awake()
		{
			Callback.Register( this );
		}

		protected virtual void OnEnable()
		{
			ClassInfo = Library.Database.Get( GetType() );
			titleContent = new GUIContent( ClassInfo.Title, ClassInfo.Help );
		}

		private void OnDestroy()
		{
			Callback.Unregister( this );
		}

		private void CreateGUI()
		{
			if ( ClassInfo.Components.TryGet<StyleSheetAttribute>( out var style ) )
				rootVisualElement.styleSheets.Add( style.Style );

			OnCreateGUI();
			CreateMenuBar( 1 );
		}

		protected virtual void OnCreateGUI() { }

		//
		// Menu Bar
		//

		private MenuBar _menuBar;

		private void CreateMenuBar( int pos = 1 )
		{
			_menuBar = new MenuBar( pos );
			rootVisualElement.Add( _menuBar );

			OnMenuBarCreated( _menuBar );

			var helpMenu = new GenericMenu();
			var helpUrl = ClassInfo.Components.Get<HelpURLAttribute>()?.URL ?? "https://github.com/Woosh-Crew/Espionage.Engine/wiki";
			helpMenu.AddItem( new GUIContent( "Wiki" ), false, () => Application.OpenURL( helpUrl ) );
			_menuBar.Add( "Help", helpMenu );
		}

		protected virtual void OnMenuBarCreated( MenuBar bar )
		{

		}
	}
}
