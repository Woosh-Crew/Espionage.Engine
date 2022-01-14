using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace Espionage.Engine.Editor.Internal
{
	[StyleSheet( "Assets/Espionage.Engine/Editor/Code/Elements/MenuBar.uss" )]
	public class MenuBar : Element
	{
		/// <param name="position"> 0 = Top, 1 = Bottom </param>
		public MenuBar( int position = 0, params Button[] buttons )
		{
			AddToClassList( "Menu-Bar" );
			AddToClassList( position is 1 ? "Bottom" : "Top" );
		}

		public void Add( string title, GenericMenu menu = null )
		{
			var button = new Button( () => menu?.ShowAsContext() );
			button.AddToClassList( "Menu-Bar-Button" );
			button.text = title;
			Add( button );
		}
	}
}
