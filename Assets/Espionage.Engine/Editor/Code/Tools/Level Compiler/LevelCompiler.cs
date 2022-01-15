using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Espionage.Engine.Editor.Internal
{
	[Library( "tool.level_compiler", Title = "Level Compiler", Help = "Compiles a Level for use in-game" )]
	[Icon( EditorIcons.Build ), HelpURL( "https://github.com/Woosh-Crew/Espionage.Engine/wiki" )]
	public sealed class LevelCompiler : Tool
	{
		[MenuItem( "Tools/Level Compiler _F8", false, -150 )]
		private static void ShowEditor()
		{
			var wind = EditorWindow.GetWindow<LevelCompiler>();
		}

		protected override void OnCreateGUI()
		{
		}

		protected override void OnMenuBarCreated( MenuBar bar )
		{
			bar.Add( "File", null );
			bar.Add( "Edit", null );
			bar.Add( "View", null );
		}
	}
}
