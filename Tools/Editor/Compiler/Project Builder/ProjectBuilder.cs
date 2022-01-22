using System.IO;
using System.Collections;
using System.Collections.Generic;
using Espionage.Engine.Editor;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


namespace Espionage.Engine.Tools.Editor
{
	[Title( "Project Builder" ), Group( "Compiler" ), Icon( EditorIcons.Code ), HelpURL( "https://github.com/Woosh-Crew/Espionage.Engine/wiki" ), StyleSheet( GUID = "286338582a0f405dad4fcb85ab99dcc7" )]
	public class ProjectBuilder : Tool
	{
		[MenuItem( "Tools/Project Builder _F5", false, -150 )]
		private static void ShowEditor()
		{
			GetWindow<ProjectBuilder>();
		}

		protected override void OnCreateGUI()
		{
			// Header
			rootVisualElement.Add( new HeaderBar( "Project Builder", "Builds the project to the target platform.", new Image() { image = ClassInfo.Components.Get<IconAttribute>().Icon }, "Header-Bottom-Border" ) );

			// Build Preset
			{
				var box = new VisualElement();
				box.AddToClassList( "Box" );
				rootVisualElement.Add( box );

				box.Add( new Label( $"Target Game - {Engine.Game?.ClassInfo.Name ?? "None"}" ) );

				if ( Engine.Game is null )
				{
					box.Add( new Label( $"Please create a game in order to build" ) );
				}
			}

			if ( Engine.Game is null )
			{
				return;
			}

			// Meta
			rootVisualElement.Add( new TitleBar( $"Meta Data - [From: {Engine.Game.ClassInfo.Name} / {Engine.Game.ClassInfo.Title}]", null, "Bottom", "Top" ) );
			{
				var box = new VisualElement();
				box.AddToClassList( "Box" );
				rootVisualElement.Add( box );

				box.Add( new TextField( "Product" ) { isReadOnly = true, value = Application.productName } );
				box.Add( new TextField( "Company" ) { isReadOnly = true, value = Application.companyName } );
				box.Add( new TextField( "Version" ) { isReadOnly = true, value = Application.version } );

				box.Add( new ObjectField( "Splash Screen" )
				{
					objectType = typeof( SceneAsset ),
					tooltip = "Splash Screen is used for Loading Assets and hiding Initialization",
					value = AssetDatabase.LoadAssetAtPath<SceneAsset>( Engine.Game?.SplashScreen )
				} );

				box.Add( new ObjectField( "Main Menu" )
				{
					objectType = typeof( SceneAsset ),
					tooltip = "Main Menu is loaded after the Splash Screen",
					value = AssetDatabase.LoadAssetAtPath<SceneAsset>( Engine.Game?.MainMenu )
				} );
			}

			// Post Build
			rootVisualElement.Add( new TitleBar( "Post Build", null, "Bottom", "Top" ) );
			{
				var box = new VisualElement();
				box.AddToClassList( "Box" );
				rootVisualElement.Add( box );

				box.Add( new Toggle( "Launch Game" ) );
				box.Add( new Toggle( "Upload to Steam" ) );
			}

			// Build
			rootVisualElement.Add( new TitleBar( "Build", null, "Bottom", "Top" ) );
			{
				var box = new VisualElement();
				box.AddToClassList( "Box" );
				rootVisualElement.Add( box );

				box.Add( new Button( () => Build( BuildTarget.StandaloneWindows, BuildOptions.None ) ) { text = "Build Game" } );
				box.Add( new Button( () => Build( BuildTarget.StandaloneWindows, BuildOptions.None ) ) { text = "Build Server" } );
			}
		}

		public static void Build( BuildTarget target, BuildOptions options )
		{
			using ( Debugging.Stopwatch( "Project Build Finished", true ) )
			{
				// Original Scene
				var originalScene = SceneManager.GetActiveScene().path;
				var scene = EditorSceneManager.NewScene( NewSceneSetup.DefaultGameObjects, NewSceneMode.Single );

				//
				// Build Game
				//

				try
				{
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
						scenes = new[]
						{
							"Assets/Espionage.Engine.Cache/Preload.unity",
							Engine.Game.SplashScreen,
							Engine.Game.MainMenu
						},
						locationPathName = $"Exports/{PlayerSettings.productName} {PlayerSettings.bundleVersion}/{PlayerSettings.productName}.exe",
						options = options,
						target = target,
						targetGroup = BuildTargetGroup.Standalone
					};

					Callback.Run( "project_builder.building", target );
					BuildPipeline.BuildPlayer( buildSettings );
				}
				finally
				{
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
				}

				//
				// Create Content Directory
				//

				var dataPath = $"Exports/{PlayerSettings.productName} {PlayerSettings.bundleVersion}/{PlayerSettings.productName}_Data";

				//
				// Move Levels
				//

				const string exportedMapsPath = "Exports/Maps/";
				if ( Directory.Exists( exportedMapsPath ) )
				{
					var mapsPath = $"{dataPath}/Maps";
					if ( !Directory.Exists( mapsPath ) )
					{
						Directory.CreateDirectory( mapsPath );
					}

					var levelFiles = Directory.GetFiles( exportedMapsPath, "*.map", SearchOption.AllDirectories );
					foreach ( var item in levelFiles )
					{
						var name = Path.GetFileName( item );
						Debugging.Log.Info( $"Moving {name}, to exported project" );
						File.Copy( item, $"{mapsPath}/{name}" );
					}
				}
			}
		}
	}
}
