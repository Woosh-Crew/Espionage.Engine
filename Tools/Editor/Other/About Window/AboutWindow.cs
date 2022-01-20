using System.Collections;
using System.Collections.Generic;
using Espionage.Engine.Editor;
using UnityEngine;
using UnityEditor;

namespace Espionage.Engine.Tools.Editor
{
	[Library( Title = "About Window", Group = "Hidden" )]
	public class AboutWindow : Tool
	{
		protected override MenuBar.Position MenuBarPosition => MenuBar.Position.None;

		public static void ShowWindow()
		{
			var wind = CreateInstance<AboutWindow>();
			var size = new Vector2( 450, 250 );

			wind.position = new Rect( new Vector2( Screen.width / 2 + size.x / 2, Screen.height / 2 - size.y / 2 ), size );
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
