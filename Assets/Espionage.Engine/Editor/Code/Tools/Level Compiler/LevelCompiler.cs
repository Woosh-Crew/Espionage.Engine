using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Threading.Tasks;
using System.Linq;

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

			QuickCompilerUI();
			AdvancedCompilerUI();
		}

		private void QuickCompilerUI()
		{
			var texture = AssetDatabase.LoadAssetAtPath<Texture>( EditorIcons.Map );
			var icon = new Image() { image = texture };
			var title = new TitleBar( "Quick Compile", icon, "Bottom" );

			rootVisualElement.Add( title );

			// Quick Buttons
			{
				var getOpenScene = new Button( () => Compile( EditorSceneManager.GetActiveScene(), BuildTarget.StandaloneWindows ) ) { text = "Quick Compile Open Scene" };
				rootVisualElement.Add( getOpenScene );
			}
		}

		private void AdvancedCompilerUI()
		{
			var texture = AssetDatabase.LoadAssetAtPath<Texture>( EditorIcons.Construct );
			var icon = new Image() { image = texture };
			var title = new TitleBar( "Advanced Compile", icon, "Top", "Bottom" );

			rootVisualElement.Add( title );

			// Target
			{
				var getOpenScene = new Button() { text = "Get Active Scene" };
				rootVisualElement.Add( getOpenScene );

				var field = new ObjectField( "Target Scene" );
				field.objectType = typeof( SceneAsset );
				field.RegisterValueChangedCallback( ( e ) => { Target = e.newValue as SceneAsset; } );

				rootVisualElement.Add( field );
			}
		}

		//
		// Level Logic
		//

		// Target Level

		private SceneAsset _target;
		public SceneAsset Target
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

		public void OnBlueprintChange( SceneAsset oldScene, SceneAsset newScene )
		{
			Callback.Run( "compiler.target_changed", oldScene, newScene );
		}

		public static bool Compile( Scene scene, params BuildTarget[] buildTargets )
		{
			// Ask the user if they want to save the scene, if not don't export!
			if ( !EditorSceneManager.SaveModifiedScenesIfUserWantsTo( new Scene[] { scene } ) )
				return false;

			var exportPath = $"Exports/Levels/{scene.name}/";

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

				// Create the Level1 scene, we use this for preprocessing & exporting
				EditorSceneManager.SaveScene( scene, "Assets/Level1.unity", true );

				AssetDatabase.Refresh();
				var levelAsset = AssetImporter.GetAtPath( "Assets/Level1.unity" );

				if ( !Directory.Exists( Path.GetFullPath( exportPath ) ) )
					Directory.CreateDirectory( Path.GetFullPath( exportPath ) );

				// For each target build, build
				foreach ( var target in buildTargets )
				{
					var bundleName = $"{scene.name}.lvl" + (target == BuildTarget.StandaloneWindows ? "w" : "m");
					levelAsset.assetBundleName = bundleName;
					var bundle = BuildPipeline.BuildAssetBundles( exportPath, BuildAssetBundleOptions.ChunkBasedCompression, target );

					if ( bundle is null )
					{
						EditorUtility.DisplayDialog( "ERROR", $"Map asset bundle compile failed. {target.ToString()}", "Okay" );
						return false;
					}
				}

				// Once we are done do any cleanup on scene
				EditorSceneManager.OpenScene( scene.path );

				Callback.Run( "compiler.post_process", scene );

				EditorSceneManager.SaveScene( scene );

				// Remove all bundle names
				foreach ( var item in AssetDatabase.GetAllAssetBundleNames() )
					AssetDatabase.RemoveAssetBundleName( item, true );

				// Delete Level1, as its not needed anymore
				AssetDatabase.DeleteAsset( "Assets/Level1.unity" );
				AssetDatabase.Refresh();
			}

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
