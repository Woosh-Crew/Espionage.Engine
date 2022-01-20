using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;
using System.IO;
using UnityEditor.UIElements;

namespace Espionage.Engine.Editor.Internal
{
	[Title( "Project Builder" ), Group( "Compiler" ), Icon( EditorIcons.Code ), HelpURL( "https://github.com/Woosh-Crew/Espionage.Engine/wiki" )]
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

			// Header
			rootVisualElement.Add( new HeaderBar( "Project Builder", "Builds the project to the target platform.", new Image() { image = ClassInfo.Components.Get<IconAttribute>().Icon }, "Header-Bottom-Border" ) );

			rootVisualElement.Add( new TitleBar( "Meta Data", null, "Bottom" ) );

			rootVisualElement.Add( new Label( Application.productName ) );
			rootVisualElement.Add( new Label( Application.companyName ) );
			rootVisualElement.Add( new Label( Application.version ) );

			// Scenes
			rootVisualElement.Add( new TitleBar( "Scenes", null, "Bottom", "Top" ) );
			rootVisualElement.Add( new ObjectField( "Splash Screen" ) { objectType = typeof( SceneAsset ), tooltip = "Splash Screen is used for Loading Assets and hiding Initialization" } );
			rootVisualElement.Add( new ObjectField( "Main Menu" ) { objectType = typeof( SceneAsset ), tooltip = "Main Menu is loaded after the Splash Screen" } );

			// Post Build
			rootVisualElement.Add( new TitleBar( "Post Build", null, "Bottom", "Top" ) );
			rootVisualElement.Add( new Toggle( "Launch Game" ) );
			rootVisualElement.Add( new Toggle( "Upload to Steam" ) );

			// Build
			rootVisualElement.Add( new TitleBar( "Build", null, "Bottom", "Top" ) );
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
				if ( string.IsNullOrEmpty( originalScene ) )
				{
					EditorSceneManager.NewScene( NewSceneSetup.DefaultGameObjects, NewSceneMode.Single );
				}
				else
				{
					EditorSceneManager.OpenScene( originalScene, OpenSceneMode.Single );
				}

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
