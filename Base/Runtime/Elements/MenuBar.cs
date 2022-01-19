using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace Espionage.Engine.Editor.Internal
{
	[StyleSheet( "Assets/Espionage.Engine/Base/Runtime/Elements/MenuBar.uss" )]
	public class MenuBar : Element
	{
		public enum Position { Top, Bottom, None }

		public MenuBar( Position position, params Button[] buttons )
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
