using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Espionage.Engine.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Threading.Tasks;
using System.Linq;
using Espionage.Engine.Resources;

namespace Espionage.Engine.Tools.Editor
{
	[Library( "tool.map_compiler", Title = "Map Compiler", Help = "Compiles a Map for use in-game", Group = "Compiler" ), Icon( EditorIcons.Build ), HelpURL( "https://github.com/Woosh-Crew/Espionage.Engine/wiki" )]
	public sealed class MapCompiler : EditorTool
	{
		[MenuItem( "Tools/Map Compiler _F8", false, -150 )]
		private static void ShowEditor()
		{
			GetWindow<MapCompiler>();
		}

		protected override void OnCreateGUI()
		{
			var texture = ClassInfo.Components.Get<IconAttribute>().Icon;
			var icon = new Image() { image = texture };

			var header = new HeaderBar( ClassInfo.Title, "Select a level and press compile!", icon, "Header-Bottom-Border" );
			rootVisualElement.Add( header );

			QuickCompilerUI();
		}

		private void QuickCompilerUI()
		{
			var texture = AssetDatabase.LoadAssetAtPath<Texture>( EditorIcons.Map );
			var icon = new Image() { image = texture };
			var title = new TitleBar( "Quick Compile", icon, "Bottom" );

			rootVisualElement.Add( title );

			// Quick Buttons
			{
				var getOpenScene = new Button( () => Compile( SceneManager.GetActiveScene(), BuildTarget.StandaloneWindows ) ) { text = "Quick Compile Open Scene" };
				rootVisualElement.Add( getOpenScene );
			}
		}

		//
		// Level Logic
		//

		public static bool Compile( Scene scene, params BuildTarget[] buildTargets )
		{
			// Ask the user if they want to save the scene, if not don't export!
			if ( !EditorSceneManager.SaveModifiedScenesIfUserWantsTo( new[] { scene } ) )
			{
				return false;
			}

			var exportPath = $"Exports/{Library.Database.Get<Map>().Group}/{scene.name}/";

			// Track how long exporting took
			using ( Debugging.Stopwatch( "Level Compiled", true ) )
			{
				if ( Callback.Run<bool>( "compiler.sanity_check", scene )?.Any( e => e is false ) ?? false )
				{
					Debug.Log( "Sanity check failed" );
					return false;
				}

				// Compile Preprocess. Allows anything to act as a preprocessor
				Callback.Run( "compiler.pre_process", scene );

				try
				{
					// Create the Level1 scene, we use this for preprocessing & exporting
					EditorSceneManager.SaveScene( scene, "Assets/Map.unity", true );
					AssetDatabase.Refresh();

					if ( !Directory.Exists( Path.GetFullPath( exportPath ) ) )
					{
						Directory.CreateDirectory( Path.GetFullPath( exportPath ) );
					}

					var extension = Library.Database.Get<Map>().Components.Get<FileAttribute>().Extension ?? "map";

					var builds = new[]
					{
						new AssetBundleBuild()
						{
							assetNames = new[] { "Assets/Map.unity" },
							assetBundleName = $"{scene.name}.{extension}"
						}
					};

					// For each target build, build
					foreach ( var target in buildTargets )
					{
						var bundle = BuildPipeline.BuildAssetBundles( exportPath, builds, BuildAssetBundleOptions.ChunkBasedCompression, target );

						if ( bundle is null )
						{
							EditorUtility.DisplayDialog( "ERROR", $"Map asset bundle compile failed. {target.ToString()}", "Okay" );
							return false;
						}
					}
				}
				finally
				{
					EditorSceneManager.OpenScene( scene.path );

					// Delete Level1, as its not needed anymore
					AssetDatabase.DeleteAsset( "Assets/Map.unity" );
					AssetDatabase.Refresh();
				}
			}

			return true;
		}

		//
		// Menu Bar
		//

		protected override void OnMenuBarCreated( MenuBar bar )
		{
			// Testing
			var menu = new GenericMenu();

			menu.AddItem( new GUIContent( "Testing/Open Map" ), false, () =>
			{
				// Open Map
				var path = EditorUtility.OpenFilePanel( "Select Map", "Exports/Maps", "map" );

				if ( string.IsNullOrEmpty( path ) )
				{
					return;
				}

				if ( !EditorApplication.isPlaying )
				{
					EditorApplication.EnterPlaymode();
				}

				Debugging.Log.Info( path );
				Map.Find( Path.GetFullPath( path ) ).Load();
			} );

			menu.AddItem( new GUIContent( "Testing/Current Map" ), false, () =>
			{
				Debugging.Log.Info( Map.Current );
			} );

			menu.AddItem( new GUIContent( "Testing/Unload Map" ), false, () =>
			{
				Map.Current.Unload();
			} );

			bar.Add( "Runtime", menu );
		}
	}
}
