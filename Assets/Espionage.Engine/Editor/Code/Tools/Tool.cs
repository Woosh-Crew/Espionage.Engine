
using UnityEditor;
using UnityEngine;

namespace Espionage.Engine.Internal.Editor
{
	/// <summary> A Tool is just an EditorWindow with 
	/// ILibrary and callbacks registered </summary>
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
		}

		protected virtual void OnCreateGUI() { }
	}
}
