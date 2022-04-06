using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using System.IO;
using Espionage.Engine.Resources;
using Espionage.Engine.Resources.Editor;
using UnityEditor;
#endif

namespace Espionage.Engine.Tools.Editor
{
	[CreateAssetMenu( menuName = "Espionage.Engine/Project Builder" )]
	public class Builder : ScriptableObject
	{
#if UNITY_EDITOR

		public void Build( BuildTarget target, BuildOptions options )
		{
			if ( Engine.Game == null )
			{
				Dev.Log.Error( "No Game!" );
				return;
			}

			// Compile Default Assets
			var errorPath = "Packages/com.wooshcrew.espionage.engine/Assets/Models/Bad/w_error.prefab";
			var garryPath = "Packages/com.wooshcrew.espionage.engine/Assets/Models/Garry/w_garry.prefab";

			if ( !Files.Pathing.Exists( "models://w_garry.umdl" ) || !Files.Pathing.Exists( "models://w_error.umdl" ) )
			{
				Dev.Log.Info( "Compiling Default Resources" );
				ResourceCompiler.Compile( errorPath, typeof( GameObject ) );
				ResourceCompiler.Compile( garryPath, typeof( GameObject ) );
			}

			var info = Engine.Game.ClassInfo;
			var path = $"Exports/{info.Title}/";

			PlayerSettings.productName = info.Title;

			// Setup BuildPipeline
			var buildSettings = new BuildPlayerOptions()
			{
				locationPathName = $"{path}{info.Name}.exe",
				options = options,
				target = target,
				//
				extraScriptingDefines = new[] { "ESP_ENGINE_RUNTIME" },
				targetGroup = BuildTargetGroup.Standalone
			};

			if ( !string.IsNullOrEmpty( Engine.Game.Splash.Scene ) )
			{
				buildSettings.scenes = new[] { Engine.Game.Splash.Scene };
			}

			Callback.Run( "project_builder.building", target, buildSettings );
			BuildPipeline.BuildPlayer( buildSettings );

			MoveCompiledAssets( $"{path}{info.Name}_Data/" );
		}

		public void Play( string launchArgs = null )
		{
			var info = Engine.Game.ClassInfo;
			Process.Start( Files.Pathing.Absolute( $"Exports/{info.Title}/{info.Name}.exe" ), launchArgs );
		}

		// Utility

		private void MoveCompiledAssets( string path )
		{
			path = Files.Pathing.Absolute( path );

			foreach ( var library in Library.Database.GetAll<IResource>() )
			{
				// Does Assets actually Exists?
				if ( !Files.Pathing.Exists( $"assets://{library.Group}" ) )
				{
					Dev.Log.Info( $"{library.Title} doesn't have any exported assets." );
					continue;
				}

				var outputPath = $"{path}{library.Group}/";
				Files.Pathing.Create( outputPath );

				foreach ( var file in Files.Pathing.All( $"assets://{library.Group}" ) )
				{
					Files.Copy( file, outputPath );
				}
			}
		}

#endif

		// Fields

		[SerializeField]
		private bool master;
	}

	#if UNITY_EDITOR

	[CustomEditor( typeof( Builder ) )]
	public class BuilderEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if ( GUILayout.Button( "Build" ) )
			{
				((Builder)target).Build( BuildTarget.StandaloneWindows64, BuildOptions.None );
			}

			if ( GUILayout.Button( "Test" ) )
			{
				((Builder)target).Play();
			}
		}
	}

	#endif
}
