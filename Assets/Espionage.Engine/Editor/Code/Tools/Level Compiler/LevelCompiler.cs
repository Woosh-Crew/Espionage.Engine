using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
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

			// Compiler Logic

			var compileButton = new Button() { text = "Compile Open Scene" };
			compileButton.clicked += () =>
			{
				var level = Selection.activeObject as Level;

				if ( level is null )
					return;

				Compile( level, BuildTarget.StandaloneWindows );
			};

			rootVisualElement.Add( compileButton );
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

		public static bool Compile( Level level, params BuildTarget[] buildTargets )
		{
			var scenePath = AssetDatabase.GetAssetPath( level.scene );
			var scene = SceneManager.GetSceneByPath( scenePath );

			// Ask the user if they want to save the scene, if not don't export!
			if ( !EditorSceneManager.SaveModifiedScenesIfUserWantsTo( new Scene[] { scene } ) )
				return false;

			// Copies the scene to Level01 cause of intruder stupid shit
			EditorSceneManager.SaveScene( scene, "Assets/Level1.unity", true );

			// Give the level01 asset
			var levelAsset = AssetImporter.GetAtPath( "Assets/Level1.unity" );

			// Check if the dir is there and export level
			var exportPath = $"Exports/Maps/{scene.name}/";


			if ( !Directory.Exists( Path.GetFullPath( exportPath ) ) )
				Directory.CreateDirectory( Path.GetFullPath( exportPath ) );

			// FileUtility.DirectoryCheck( $"Exports/Maps/{scene.name}/" );

			// For each target build, build
			foreach ( var target in buildTargets )
			{
				levelAsset.assetBundleName = "map.lvl" + (target == BuildTarget.StandaloneWindows ? "w" : "m");
				var bundle = BuildPipeline.BuildAssetBundles( exportPath, BuildAssetBundleOptions.ChunkBasedCompression, target );

				if ( bundle == null )
				{
					EditorUtility.DisplayDialog( "ERROR", $"Map asset bundle compile failed. {target.ToString()}", "Okay" );
					Debug.LogError( "Compile Failed" );
					return false;
				}
			}

			AssetDatabase.Refresh();
			EditorUtility.DisplayDialog( $"Successfully Compiled {level.title}", $"For a full build report, look in the console", "Okay!" );

			return true;
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
