using System;
using UnityEngine.UIElements;

namespace Espionage.Engine.Editor.Internal
{
	[StyleSheet( "Assets/Espionage.Engine/Editor/Code/Elements/MenuBar.uss" )]
	public class MenuBar : Element
	{
		public MenuBar()
		{
			AddToClassList( "Menu-Bar" );
		}

		public Button AddMenuButton( string title, Action onClicked = null )
		{
			var button = new Button( onClicked ) { text = title };
			button.AddToClassList( "Menu-Bar-Button" );
			Add( button );
			return button;
		}
	}
}
