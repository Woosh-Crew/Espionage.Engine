using System.IO;
using System.Linq;
using Espionage.Engine.Editor;
using Espionage.Engine.Resources;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Espionage.Engine.Tools.Editor
{
	[Title( "Project Builder" ), Group( "Compiler" ), StyleSheet( GUID = "286338582a0f405dad4fcb85ab99dcc7" )]
	public class ProjectBuilder : EditorTool
	{
		[MenuItem( "Tools/Espionage.Engine/Project Builder _F5", priority = -25 )]
		private static void ShowEditor()
		{
			GetWindow<ProjectBuilder>();
		}

		protected override void OnCreateGUI()
		{
			// Header
			rootVisualElement.Add( new HeaderBar( "Project Builder", "Builds the project to the target platform.", new()
			{
				// image = ClassInfo.Components.Get<IconAttribute>().Icon
			}, "Header-Bottom-Border" ) );

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
					objectType = typeof( SceneAsset ), tooltip = "Splash Screen is used for Loading Assets and hiding Initialization"
					// value = AssetDatabase.LoadAssetAtPath<SceneAsset>( Engine.Game?.SplashScreen )
				} );

				box.Add( new ObjectField( "Main Menu" )
				{
					objectType = typeof( SceneAsset ), tooltip = "Main Menu is loaded after the Splash Screen"
					// value = AssetDatabase.LoadAssetAtPath<SceneAsset>( Engine.Game?.MainMenu )
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

				box.Add( new Button( () => Build( BuildTarget.StandaloneWindows64, BuildOptions.Development ) ) { text = "Build Game" } );
			}
		}

		public static void Build( BuildTarget target, BuildOptions options )
		{
			using ( Dev.Stopwatch( "Project Build Finished", true ) )
			{
				try
				{
					var blueprintBuild = new AssetBundleBuild
					{
						assetNames = Library.Database.GetAll<Prefab>().Select( e => e.Components.Get<FileAttribute>()?.Path ).ToArray(), assetBundleName = $"{Library.Database.Get<Prefab>().Title}.pak"
					};

					// Setup BuildPipeline
					var buildSettings = new BuildPlayerOptions()
					{
						// scenes = new[] { Engine.Game.SplashScreen, Engine.Game.MainMenu },
						locationPathName = $"Exports/{PlayerSettings.productName} {PlayerSettings.bundleVersion}/{PlayerSettings.productName}.exe",
						options = options,
						target = target,
						targetGroup = BuildTargetGroup.Standalone
					};

					Callback.Run( "project_builder.building", target, buildSettings );
					var report = BuildPipeline.BuildPlayer( buildSettings );
				}
				finally
				{
					AssetDatabase.Refresh();
				}

				//
				// Move Content to Game
				//

				// TODO: Need to add support for having a deep clean project build. Where it clears all old content.

				var dataPath = $"Exports/{PlayerSettings.productName} {PlayerSettings.bundleVersion}/{PlayerSettings.productName}_Data";

				foreach ( var library in Library.Database.GetAll<IAsset>().Where( e => !e.Class.IsAbstract ) )
				{
					if ( !library.Components.TryGet<FileAttribute>( out var file ) )
					{
						Dev.Log.Warning( $"{library.Name} does have file component. Not moving " );
						continue;
					}

					var exportedPath = $"Exports/{library.Group}/";
					var destinationPath = $"{dataPath}/{library.Group}";

					// Create the built games, asset dir
					if ( !Directory.Exists( destinationPath ) )
					{
						Directory.CreateDirectory( destinationPath );
					}

					// if we have any compiled assets of that type, copy them
					if ( Directory.Exists( exportedPath ) )
					{
						var levelFiles = Directory.GetFiles( exportedPath, $"*.{file.Extension}", SearchOption.AllDirectories );

						foreach ( var item in levelFiles )
						{
							var name = Path.GetFileName( item );
							Dev.Log.Info( $"Moving {name}, to exported project" );
							File.Copy( item, $"{destinationPath}/{name}", true );
						}
					}
				}
			}
		}
	}
}
