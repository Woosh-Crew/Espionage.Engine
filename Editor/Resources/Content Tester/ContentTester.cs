using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Espionage.Engine.Editor;
using Espionage.Engine.Resources;
using Espionage.Engine.Resources.Editor;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Tools.Editor
{
	[Title( "Content Tester" ), Help( "Test Resources such as Maps, Models, Textures, etc." )]
	public class ContentTester : EditorTool
	{
		// Menu Items

		[MenuItem( "Tools/Espionage.Engine/Content Tester _F7", priority = -25 )]
		private static void ShowEditor()
		{
			GetWindow<ContentTester>();
		}

		[Function, Menu( "Maps/Open Map" )]
		private void OpenMap()
		{
			if ( !Application.isPlaying )
			{
				Debugging.Log.Warning( "Can't Load map while not in Play Mode" );
				return;
			}

			// Get all Map Types and their extensions
			var providers = Library.Database.GetAll<IFile<Map, Scene>>().Where( e => e.Components.Get<FileAttribute>() != null );
			var path = EditorUtility.OpenFilePanel( "Load a Map", "Exports/Maps", string.Join( ',', providers.Select( e => e.Components.Get<FileAttribute>().Extension ) ) );

			if ( string.IsNullOrEmpty( path ) )
			{
				Debugging.Log.Info( "No Map Selected" );
				return;
			}

			Map.Find( path ).Load();
		}

		[Function, Menu( "Maps/Test Open Map" )]
		private void TestOpenMap()
		{
			Debugging.Log.Info( "This would do something" );
			Debugging.Log.Info( Files.Path( "maps://map_testing.umap" ) );
			Process.Start( @"E:\Projects\Espionage\Espionage.Engine\Exports\Espionage 0.1\Espionage.exe", Files.Path( @"E:\Projects\Netscape\Exports\Maps\MovementArea\movementarea.umap" ) );
		}

		[Function, Menu( "Maps/Compile Map" )]
		private void OpenMapCompiler()
		{
			GetWindow<MapCompiler>();
		}

		// UI

		protected override void OnCreateGUI()
		{
			var header = new HeaderBar( ClassInfo.Title, ClassInfo.Help, null, "Header-Bottom-Border" );
			rootVisualElement.Add( header );
		}
	}
}
