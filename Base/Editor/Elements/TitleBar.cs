using UnityEngine.UIElements;
using UnityEditor;

namespace Espionage.Engine.Editor.Internal
{
	[StyleSheet( GUID = "9c815de4a74e979499b715c9567688db" )]
	public class TitleBar : Element
	{
		public TitleBar( string title, Image icon, params string[] classes )
		{
			// Icon
			if ( icon is not null )
			{
				_icon = icon;
				_icon.AddToClassList( "Icon" );
				Add( _icon );
			}

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
			set => _title.text = value;
		}

		//
		// UI
		//

		private Label _title;
		private Image _icon;
	}
}
