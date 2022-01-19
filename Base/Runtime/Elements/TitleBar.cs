using UnityEngine.UIElements;
using UnityEditor;

namespace Espionage.Engine.Editor.Internal
{
	[StyleSheet( "Assets/Espionage.Engine/Editor/Code/Elements/TitleBar.uss" )]
	public class TitleBar : Element
	{
		public TitleBar( string title, Image icon, params string[] classes )
		{
			// Icon
			_icon = icon;
			_icon.AddToClassList( "Icon" );
			Add( _icon );

			_title = new Label( title );
			_title.AddToClassList( "Title" );
			Add( _title );

			foreach ( var item in classes )
			{
				AddToClassList( item );
			}
		}

		//
		// Exposed API
		//

		public Image Icon
		{
			set
			{
				_icon.RemoveFromHierarchy();
				Add( value );
			}
		}

		public string Title
		{
			set
			{
				_title.text = value;
			}
		}

		//
		// UI
		//

		private Label _title;
		private Image _icon;
	}
}
