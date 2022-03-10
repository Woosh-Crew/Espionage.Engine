using Espionage.Engine.Editor;
using Espionage.Engine.Tools.Editor;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources.Editor
{
	[Title( "Map Compiler" ), Help( "Compile your maps, to the .umap format." ), Group( "Compiler" )]
	internal class MapCompiler : EditorTool, IHasCustomMenu
	{
		protected override void OnCreateGUI()
		{
			rootVisualElement.Add( new HeaderBar( ClassInfo.Title, ClassInfo.Help, null, "Header-Bottom-Border" ) );
		}

		public void AddItemsToMenu( GenericMenu menu )
		{
			menu.AddItem( new( "Quick Compile Map" ), false, CompileActiveScene );
		}

		// Menu Items

		[MenuItem( "Tools/Espionage.Engine/Compiler/Map Compiler", priority = 2 )]
		public static void OpenWindow()
		{
			GetWindow<MapCompiler>();
		}

		[MenuItem( "Tools/Espionage.Engine/Compiler/Quick Compile Map _F6" )]
		public static void CompileActiveScene()
		{
			UMAP.Compile( SceneManager.GetActiveScene().path, BuildTarget.StandaloneWindows );
		}

		[Function, Menu( "File/Open Scene" )]
		private void OpenScene()
		{
			Debugging.Log.Info( "Opening Scene" );
		}
	}
}
