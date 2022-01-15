using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
			var texture = ClassInfo.Components.Get<IconAttribute>().Icon;
			var icon = new Image() { image = texture };

			var header = new HeaderBar( ClassInfo.Title, "Select a level and press compile!", icon, "Header-Bottom-Border" );
			rootVisualElement.Add( header );
		}

		//
		// Level Logic
		//

		// Target Level

		private Level _target;
		public Level Target
		{
			get
			{
				return _target;
			}
			set
			{
				OnBlueprintChange( _target, value );
				_target = value;
			}
		}

		public Action<Level, Level> OnTargetChanged;
		public void OnBlueprintChange( Level oldBp, Level newBp )
		{
			OnTargetChanged?.Invoke( oldBp, newBp );
		}

		//
		// Menu Bar
		//

		protected override void OnMenuBarCreated( MenuBar bar )
		{
			bar.Add( "File", null );
			bar.Add( "Edit", null );
			bar.Add( "View", null );
		}
	}
}
