using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;
using System.IO;

namespace Espionage.Engine.Editor.Internal
{
	[Title( "Project Builder" )]
	[Group( "Compiler" )]
	[Icon( EditorIcons.Terminal )]
	[HelpURL( "https://github.com/Woosh-Crew/Espionage.Engine/wiki" )]
	public class ProjectBuilder : Tool
	{
		[MenuItem( "Tools/Project Builder _F5", false, -150 )]
		private static void ShowEditor()
		{
			var wind = GetWindow<ProjectBuilder>();
		}

		protected override void OnCreateGUI()
		{
			base.OnCreateGUI();

			rootVisualElement.Add( new Button( () => Build( BuildTarget.StandaloneWindows, BuildOptions.None ) ) { text = "Build Project" } );
		}

		public static void Build( BuildTarget target, BuildOptions options )
		{
			using ( Debugging.Stopwatch( "Project Build Finished", true ) )
			{
				// Original Scene
				var originalScene = SceneManager.GetActiveScene().path;

				var scene = EditorSceneManager.NewScene( NewSceneSetup.DefaultGameObjects, NewSceneMode.Single );

				// Create the Cache Dir if it doesnt exist
				if ( !Directory.Exists( Path.GetFullPath( "Assets/Espionage.Engine.Cache/" ) ) )
				{
					Directory.CreateDirectory( Path.GetFullPath( "Assets/Espionage.Engine.Cache/" ) );
				}

				// Save the preload scene so we can export with it
				EditorSceneManager.SaveScene( scene, "Assets/Espionage.Engine.Cache/Preload.unity" );
				SceneManager.SetActiveScene( scene );

				// Setup BuildPipeline
				var buildSettings = new BuildPlayerOptions()
				{
					scenes = new string[] { "Assets/Espionage.Engine.Cache/Preload.unity" },
					locationPathName = $"Exports/{PlayerSettings.productName}/{PlayerSettings.productName}.exe",
					options = options,
					target = target,
					targetGroup = BuildTargetGroup.Standalone
				};


				Callback.Run( "project_builder.building", target );
				BuildPipeline.BuildPlayer( buildSettings );

				// Load back into original scene, in case IO throws an exception
				EditorSceneManager.OpenScene( originalScene, OpenSceneMode.Single );

				// Delete Cache
				AssetDatabase.DeleteAsset( "Assets/Espionage.Engine.Cache" );
				AssetDatabase.Refresh();

				// Move all exported content to Game
				// Create the Cache Dir if it doesnt exist
				var contentPath = $"Exports/{PlayerSettings.productName}/{PlayerSettings.productName}_Content/";
				if ( Directory.Exists( contentPath ) )
				{
					Directory.Delete( contentPath, true );
				}

				Directory.CreateDirectory( contentPath );

				// LEVEL
				// Create the Cache Dir if it doesnt exist
				Directory.CreateDirectory( $"{contentPath}Levels" );

				var levelFiles = Directory.GetFiles( "Exports/Levels/", "*.lvlw", SearchOption.AllDirectories );
				foreach ( var item in levelFiles )
				{
					var name = Path.GetFileName( item );
					Debugging.Log.Info( $"Moving {name}, to exported project" );
					File.Copy( item, contentPath + "Levels/" + name );
				}
			}
		}
	}
}