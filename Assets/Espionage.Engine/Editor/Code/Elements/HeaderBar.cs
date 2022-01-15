using UnityEngine.UIElements;
using UnityEditor;

namespace Espionage.Engine.Editor.Internal
{
	[StyleSheet( "Assets/Espionage.Engine/Editor/Code/Elements/MenuBar.uss" )]
	public class HeaderBar : Element
	{
		public HeaderBar( string name, string subtitle, Image icon )
		{

		}

		//
		// Exposed API
		//

		public string Title
		{
			set
			{

			}
		}

		public string SubTitle
		{
			set
			{

			}
		}


		//
		// UI
		//

		private Label _title;
		private Label _subtitle;
		private Image _icon;

		public void SetTitle( string text )
		{

		}

		public void SetSubTitle( string text )
		{

		}

		public void SetIcon( Image icon )
		{

		}
	}
}
