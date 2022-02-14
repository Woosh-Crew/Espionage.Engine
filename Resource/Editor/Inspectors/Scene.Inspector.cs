using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

namespace Espionage.Engine.Resources.Editor
{
	[CustomEditor( typeof( SceneAsset ) )]
	public class SceneInspector : UnityEditor.Editor, IHasCustomMenu
	{
		public override void OnInspectorGUI()
		{
			OnCompileGUI();
		}

		private void OnCompileGUI()
		{
			EditorGUILayout.LabelField( "Compiler", EditorStyles.boldLabel );

			GUILayout.Space( 4 );

			if ( GUILayout.Button( "Compile", Styles.CompileButton, GUILayout.Height( 28 ) ) )
			{ 
				SceneCompiler.Compile( AssetDatabase.GetAssetPath( target ), BuildTarget.StandaloneWindows );
			}
		}
		
		private static class Styles
		{
			public static readonly GUIStyle Container = new() { padding = new RectOffset( 12, 12, 12, 12 ) };
			public static readonly GUIStyle Group = new( GUI.skin.window ) { padding = new RectOffset( 8, 8, 8, 8 ) };
			public static readonly GUIStyle CompileButton = new( "Button" );
			public static readonly GUIStyle TitleField = new( EditorStyles.textField ) { alignment = TextAnchor.MiddleLeft };
		}

		public void AddItemsToMenu( GenericMenu menu )
		{
			menu.AddItem( new GUIContent( "Compile Scene" ), false, () => { SceneCompiler.Compile( AssetDatabase.GetAssetPath( target ), BuildTarget.StandaloneWindows ); } );
		}
	}
}
