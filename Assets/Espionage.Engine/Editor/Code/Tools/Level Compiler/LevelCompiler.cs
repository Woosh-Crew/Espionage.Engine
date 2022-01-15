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
				Compile( EditorSceneManager.GetActiveScene(), BuildTarget.StandaloneWindows );
			};

			rootVisualElement.Add( compileButton );
		}

		//
		// Level Logic
		//

		// Target Level

		private Scene _target;
		public Scene Target
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

		public Action<Scene, Scene> OnTargetChanged;
		public void OnBlueprintChange( Scene oldScene, Scene newScene )
		{
			OnTargetChanged?.Invoke( oldScene, newScene );
		}

		public static bool Compile( Scene scene, params BuildTarget[] buildTargets )
		{
			// Ask the user if they want to save the scene, if not don't export!
			if ( !EditorSceneManager.SaveModifiedScenesIfUserWantsTo( new Scene[] { scene } ) )
				return false;

			// Check if the dir is there and export level
			var exportPath = $"Exports/Maps/{scene.name}/";

			using ( Debugging.Stopwatch( "Level Compiled", true ) )
			{
				// Copies the scene to Level01 cause of intruder stupid shit
				EditorSceneManager.SaveScene( scene, "Assets/Level1.unity", true );

				// Give the level01 asset
				var levelAsset = AssetImporter.GetAtPath( "Assets/Level1.unity" );

				if ( !Directory.Exists( Path.GetFullPath( exportPath ) ) )
					Directory.CreateDirectory( Path.GetFullPath( exportPath ) );

				// FileUtility.DirectoryCheck( $"Exports/Maps/{scene.name}/" );

				// For each target build, build
				foreach ( var target in buildTargets )
				{
					levelAsset.assetBundleName = $"{scene.name}.lvl" + (target == BuildTarget.StandaloneWindows ? "w" : "m");
					var bundle = BuildPipeline.BuildAssetBundles( exportPath, BuildAssetBundleOptions.ChunkBasedCompression, target );

					if ( bundle == null )
					{
						EditorUtility.DisplayDialog( "ERROR", $"Map asset bundle compile failed. {target.ToString()}", "Okay" );
						Debug.LogError( "Compile Failed" );
						return false;
					}
				}

				AssetDatabase.Refresh();

			}

			EditorUtility.DisplayDialog( $"Successfully Compiled \"{scene.name}\"", $"Outputted to [{exportPath}]\n\nFor full build report look in console", "Okay!" );
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
