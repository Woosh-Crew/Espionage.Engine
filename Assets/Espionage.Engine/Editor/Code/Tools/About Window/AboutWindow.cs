using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Espionage.Engine.Editor.Internal
{
	public class AboutWindow : Tool
	{
		protected override int MenuBarPosition => -1;

		public static void ShowWindow()
		{
			var wind = ScriptableObject.CreateInstance<AboutWindow>();
			var size = new Vector2( 500, 300 );

			wind.position = new Rect( new Vector2( (Screen.width / 2) + (size.x / 2), (Screen.height / 2) - (size.y / 2) ), size );
			wind.maxSize = size;
			wind.minSize = size;

			wind.ShowModalUtility();
		}

		protected override void OnCreateGUI()
		{
			base.OnCreateGUI();
		}
	}
}

