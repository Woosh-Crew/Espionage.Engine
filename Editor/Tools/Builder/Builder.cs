#if UNITY_EDITOR

using System.Diagnostics;
using System.Linq;
using System.Text;
using Espionage.Engine.Editor.Resources;
using Espionage.Engine.IO;
using UnityEngine;
using Espionage.Engine.Resources;
using UnityEditor;

namespace Espionage.Engine.Tools.Editor
{
	[CreateAssetMenu( menuName = "Espionage.Engine/Project Builder" )]
	public class Builder : EditorWindow
	{
		[MenuItem( "Tools/Builder" )]
		private static void ShowWindow()
		{
			GetWindow<Builder>();
		}

		private void OnEnable()
		{
			titleContent.text = "Project Builder";
		}

		private void OnGUI()
		{
			if ( GUILayout.Button( "Compile Project" ) )
			{
				Build( BuildTarget.StandaloneWindows64, BuildOptions.None );
			}

			if ( GUILayout.Button( "Play" ) )
			{
				Play();
			}

			if ( GUILayout.Button( "Regenerate Builder" ) )
			{
				CreateBatch();
			}
		}

		[InitializeOnLoadMethod]
		private static void Initialize()
		{
			if ( !Files.Pathing( "exports://full_build.bat" ).Exists() )
			{
				CreateBatch();
			}
		}

		//
		// API
		//

		private static void Build()
		{
			Build( BuildTarget.StandaloneWindows64, BuildOptions.None );
			EditorApplication.Exit( 0 );
		}

		private static void BuildDevelopment()
		{
			Build( BuildTarget.StandaloneWindows64, BuildOptions.Development );
			EditorApplication.Exit( 0 );
		}

		private static void BuildCode()
		{
			Build( BuildTarget.StandaloneWindows64, BuildOptions.BuildScriptsOnly | BuildOptions.Development );
			EditorApplication.Exit( 0 );
		}

		public static void Build( BuildTarget target, BuildOptions options )
		{
			if ( Engine.Project == null )
			{
				Debugging.Log.Error( "No Game!" );
				return;
			}

			// Compile Default Assets
			const string errorPath = "Packages/com.wooshcrew.espionage.engine/Assets/Models/Bad/w_error.prefab";
			const string garryPath = "Packages/com.wooshcrew.espionage.engine/Assets/Models/Garry/w_garry.prefab";

			if ( !Files.Pathing( "models://w_garry.umdl" ).Exists() || !Files.Pathing( "models://w_error.umdl" ).Exists() )
			{
				Debugging.Log.Info( "Compiling Default Resources" );
				Compiler.Compile( errorPath, typeof( GameObject ) );
				Compiler.Compile( garryPath, typeof( GameObject ) );
			}

			var info = Engine.Project.ClassInfo;
			var path = $"Exports/{info.Title}/";

			PlayerSettings.productName = info.Title;

			// Setup BuildPipeline
			var buildSettings = new BuildPlayerOptions()
			{
				locationPathName = $"{path}{info.Name}.exe",
				options = options,
				target = target,
				extraScriptingDefines = new[] { "ESP_ENGINE_RUNTIME" },
				targetGroup = BuildTargetGroup.Standalone
			};

			if ( !string.IsNullOrEmpty( Engine.Project.Splash.Scene ) )
			{
				buildSettings.scenes = new[] { Engine.Project.Splash.Scene };
			}

			Callback.Run( "project_builder.building", target, buildSettings );
			BuildPipeline.BuildPlayer( buildSettings );

			if ( !buildSettings.options.HasFlag( BuildOptions.BuildScriptsOnly ) )
			{
				MoveCompiledAssets( $"{path}{info.Name}_Data/" );

				// Because map doesnt inherit from IResource
				MoveGroup( typeof( Map ), $"{path}{info.Name}_Data/" );
			}
		}

		public static void Play( string launchArgs = null )
		{
			var info = Engine.Project.ClassInfo;
			Process.Start( Files.Pathing( $"Exports/{info.Title}/{info.Name}.exe" ).Absolute(), launchArgs );
		}

		// Utility

		private static void CreateBatch()
		{
			var builder = new StringBuilder( "@echo off\necho Building Project\n" );

			builder.Append( $"\"{EditorApplication.applicationPath}\" -quit -batchmode -silent-crashes " );
			builder.Append( "-logFile /../Logs/compile_output.log " );
			builder.Append( $"-projectPath \"{Files.Pathing( "project://" ).Absolute()}\" " );

			Files.Save( "serializer.string", $"{builder}-executeMethod Espionage.Engine.Tools.Editor.Builder.Build ", "exports://full_build.bat" );
			Files.Save( "serializer.string", $"{builder}-executeMethod Espionage.Engine.Tools.Editor.Builder.BuildDevelopment ", "exports://development_build.bat" );
			Files.Save( "serializer.string", $"{builder}-executeMethod Espionage.Engine.Tools.Editor.Builder.BuildCode ", "exports://code_build.bat" );
		}

		private static void MoveCompiledAssets( string path )
		{
			foreach ( var library in Library.Database.GetAll<IAsset>().Where( e => !e.Info.IsAbstract ) )
			{
				MoveGroup( library, path );
			}
		}

		private static void MoveGroup( Library lib, Pathing path )
		{
			path.Absolute();

			// Does Assets actually Exists?
			if ( !Files.Pathing( $"assets://{lib.Group}" ).Exists() )
			{
				Debugging.Log.Info( $"{lib.Title} doesn't have any exported assets." );
				return;
			}

			Pathing outputPath = $"{path}{lib.Group}/";
			outputPath.Create();
			Files.Copy( $"assets://{lib.Group}", outputPath );

			Debugging.Log.Info( $"Moving [{lib.Group}] to [{outputPath}]" );
		}

	}
}

#endif
