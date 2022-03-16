using UnityEngine.UIElements;

namespace Espionage.Engine.Editor
{
	[StyleSheet( GUID = "4f913390e11109d438024966ae758619" )]
	public class HeaderBar : Element
	{
		public new class UxmlFactory : UxmlFactory<HeaderBar> { }

		public HeaderBar() { }

		public HeaderBar( string title, string subTitle, Image icon, params string[] classes )
		{
			// Icon
			if ( icon is not null )
			{
				_icon = icon;
				_icon.AddToClassList( "Icon" );
				Add( _icon );
			}

			// Text Container
			var element = new VisualElement() { name = "Title-Container" };
			Add( element );

			_title = new Label( title );
			_title.AddToClassList( "Title" );
			element.Add( _title );

			_subTitle = new Label( subTitle );
			_subTitle.AddToClassList( "Sub-Title" );
			element.Add( _subTitle );

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

		public string SubTitle
		{
			set => _subTitle.text = value;
		}


		//
		// UI
		//

		private Label _title;
		private Label _subTitle;
		private Image _icon;
	}
}
