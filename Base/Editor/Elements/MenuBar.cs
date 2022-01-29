using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace Espionage.Engine.Editor
{
	[StyleSheet( GUID = "e00c1473ba1e95a4fac4ee7549c06e6c" )]
	public class MenuBar : Element
	{
		public enum Position { Top, Bottom, None }

		public MenuBar( Position position )
		{
			AddToClassList( "Menu-Bar" );

			if ( position is not Position.None )
			{
				AddToClassList( position is Position.Bottom ? "Bottom" : "Top" );
			}
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
