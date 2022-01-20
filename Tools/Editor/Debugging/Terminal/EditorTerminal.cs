using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Espionage.Engine.Editor;

namespace Espionage.Engine.Tools.Editor
{
	[Title( "Terminal" ), Icon( EditorIcons.Terminal ), Group( "Debug" )]
	public class EditorTerminal : Tool
	{
		protected override void OnCreateGUI()
		{
			base.OnCreateGUI();
		}

		protected override void OnMenuBarCreated( MenuBar bar )
		{
			base.OnMenuBarCreated( bar );
		}
	}
}
