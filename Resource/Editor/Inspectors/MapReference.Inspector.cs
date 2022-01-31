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

		private SerializedProperty _title;
		private SerializedProperty _description;
		private SerializedProperty _icon;
		
		private void OnEnable()
		{
			_title = serializedObject.FindProperty( "title" );
			_description = serializedObject.FindProperty( "description" );
			_icon = serializedObject.FindProperty( "icon" );
		}
		
		//
		// GUI
		//

		public override void OnInspectorGUI()
		{
			OnInfoGUI();
			
			GUILayout.Space(8);
			
			OnCompileGUI();

			base.OnInspectorGUI();
		}

		private void OnInfoGUI()
		{
			serializedObject.Update();
			
			// Properties
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.PropertyField( _icon );
				
				EditorGUILayout.BeginVertical();
				{
					_title.stringValue = EditorGUILayout.TextField( _title.stringValue );
					_description.stringValue = EditorGUILayout.TextArea( _description.stringValue, EditorStyles.textArea, GUILayout.Height(64) );
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();

			serializedObject.ApplyModifiedProperties();
			
			
		}

		private void OnCompileGUI()
		{
			using ( _ = new EditorGUI.DisabledGroupScope( !Map.CanCompile() ) )
			{
				if ( GUILayout.Button( "Compile", Styles.CompileButton, GUILayout.Height(28) ) )
				{
					Map.Compile();
				}
			}
		}
		
		//
		// GUI Styles
		//
		
		private static class Styles
		{
			public static readonly GUIStyle CompileButton = new GUIStyle( "Button" ) ;
		}
	}
}
