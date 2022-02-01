using System;
using Espionage.Engine.Resources;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Espionage.Engine.Resources.Editor
{
	[CustomEditor( typeof( MapReference ) )]
	public class MapReferenceEditor : UnityEditor.Editor
	{
		private MapReference Map => target as MapReference;

		//
		// Properties
		//

		// Meta
		private SerializedProperty _title;
		private SerializedProperty _description;
		private SerializedProperty _icon;

		// Build
		private SerializedProperty _sceneAsset;
		private SerializedProperty _buildOptions;

		private void OnEnable()
		{
			_title = serializedObject.FindProperty( "title" );
			_description = serializedObject.FindProperty( "description" );
			_icon = serializedObject.FindProperty( "icon" );

			_sceneAsset = serializedObject.FindProperty( "sceneAsset" );
			_buildOptions = serializedObject.FindProperty( "buildOptions" );
		}

		//
		// GUI
		//

		public override bool UseDefaultMargins()
		{
			return false;
		}

		public override void OnInspectorGUI()
		{
			using ( _ = new EditorGUILayout.VerticalScope( Styles.Container ) )
			{
				using ( _ = new EditorGUILayout.VerticalScope( Styles.Group ) )
				{
					OnInfoGUI();
				}

				GUILayout.Space( 8 );

				using ( _ = new EditorGUILayout.VerticalScope( Styles.Group ) )
				{
					OnCompileGUI();
				}

				GUILayout.Space( 8 );

				using ( _ = new EditorGUILayout.VerticalScope( Styles.Group ) )
				{
					OnTesterGUI();
				}
			}
		}

		private void OnInfoGUI()
		{
			serializedObject.Update();

			EditorGUILayout.LabelField( "Meta Data", EditorStyles.boldLabel );
			GUILayout.Space( 2 );

			// Properties
			EditorGUILayout.BeginHorizontal();
			{
				_icon.objectReferenceValue = (Texture2D)EditorGUILayout.ObjectField( _icon.objectReferenceValue, typeof( Texture2D ), false, GUILayout.Width( 92 ), GUILayout.Height( 90 ) );

				EditorGUILayout.BeginVertical();
				{
					_title.stringValue = EditorGUILayout.TextField( _title.stringValue, Styles.TitleField, GUILayout.Height( 24 ) );
					_description.stringValue = EditorGUILayout.TextArea( _description.stringValue, EditorStyles.textArea, GUILayout.Height( 64 ) );
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();

			serializedObject.ApplyModifiedProperties();
		}

		private void OnCompileGUI()
		{
			EditorGUILayout.LabelField( "Compiler", EditorStyles.boldLabel );
			GUILayout.Space( 2 );

			EditorGUILayout.PropertyField( _sceneAsset );
			EditorGUILayout.PropertyField( _buildOptions );

			GUILayout.Space( 4 );

			using ( _ = new EditorGUI.DisabledGroupScope( !Map.CanCompile() ) )
			{
				if ( GUILayout.Button( "Compile", Styles.CompileButton, GUILayout.Height( 28 ) ) )
				{
					Map.Compile( BuildTarget.StandaloneWindows );
				}
			}
		}

		private void OnTesterGUI()
		{
			EditorGUILayout.LabelField( "Testing", EditorStyles.boldLabel );
			GUILayout.Space( 2 );

			// GUILayout.Space( 4 );

			using ( _ = new EditorGUI.DisabledGroupScope( !Map.CanCompile() ) )
			{
				if ( GUILayout.Button( "Test", Styles.CompileButton, GUILayout.Height( 28 ) ) )
				{
					Debugging.Log.Error( "Does Nothing" );
				}
			}
		}

		//
		// GUI Styles
		//

		private static class Styles
		{
			public static readonly GUIStyle Container = new() { padding = new RectOffset( 12, 12, 12, 12 ) };
			public static readonly GUIStyle Group = new( GUI.skin.window ) { padding = new RectOffset( 8, 8, 8, 8 ) };
			public static readonly GUIStyle CompileButton = new( "Button" );
			public static readonly GUIStyle TitleField = new( EditorStyles.textField ) { alignment = TextAnchor.MiddleLeft };
		}
	}
}
