using System;
using Espionage.Engine.Internal;
using UnityEditor;
using UnityEngine;

namespace Espionage.Engine.Editor
{
	[CustomEditor( typeof( Proxy ), true )]
	public class ProxyEditor : BehaviourEditor
	{
		protected override void OnEnable()
		{
			ClassInfo = Library.Database[target.GetType()];
			EditorInjection.Titles[target.GetType()] = $"{ClassInfo.Title}";

			// Only Entities can have custom icons...
			if ( ClassInfo.Components.TryGet<IconAttribute>( out var icon ) )
			{
				EditorGUIUtility.SetIconForObject( target, icon.Icon );
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EngineGUI.Header( null, serializedObject.FindProperty( "name" ), serializedObject.FindProperty( "className" ), serializedObject.FindProperty( "disabled" ) );
			serializedObject.ApplyModifiedProperties();

			return;

			var originalRect = EditorGUILayout.GetControlRect( true, 24, GUIStyle.none );
			var rect = originalRect;
			rect.width = Screen.width;
			rect.y = 0;
			rect.x = 0;

			// Box
			EditorGUI.DrawRect( rect, new Color32( 45, 45, 45, 255 ) );

			// Label
			rect.x = 16;
			rect.width -= 16;

			var labelRect = rect;
			labelRect.x += 28;
			labelRect.width = Styles.Text.CalcSize( new( "None" ) ).x;

			GUI.Label( labelRect, $"None", Styles.Text );

			var dropdownRect = rect;
			dropdownRect.x = labelRect.x + labelRect.width + 8;
			if ( GUI.Button( dropdownRect, "Class", EditorStyles.foldoutPreDrop ) ) { }

			var iconRect = rect;
			iconRect.height -= 4;
			iconRect.y += 2;
			GUI.Label( iconRect, EditorGUIUtility.ObjectContent( null, typeof( Light ) ).image );

			// Help
			if ( ClassInfo.Components.TryGet<HelpAttribute>( out var help ) && !string.IsNullOrEmpty( help.URL ) )
			{
				var helpRect = originalRect;

				helpRect.y = 0;
				helpRect.x = Screen.width - 48 - 16;
				helpRect.width = 48;

				if ( GUI.Button( helpRect, "Help", Styles.Button ) )
				{
					Application.OpenURL( help.URL );
				}
			}

			// Underline

			rect.height = 1;
			rect.width += 16;
			rect.y -= 1;
			rect.x = 0;
			var color = new Color32( 26, 26, 26, 255 );

			// Top
			EditorGUI.DrawRect( rect, color );

			// Bottom
			rect.y += originalRect.height + 1;
			EditorGUI.DrawRect( rect, color );

			if ( ClassInfo.Components.Get<EditableAttribute>()?.Editable ?? true )
			{
				DrawPropertiesExcluding( serializedObject, "m_Script", "className" );
				serializedObject.ApplyModifiedProperties();

				if ( !string.IsNullOrEmpty( ClassInfo.Help ) )
				{
					EditorGUILayout.HelpBox( ClassInfo.Help, MessageType.None );
				}
			}
			else
			{
				GUILayout.Label( "Not Editable", EditorStyles.miniBoldLabel );
			}
		}

		private static class Styles
		{
			public static readonly GUIStyle Text = new( EditorStyles.largeLabel )
			{
				fontStyle = FontStyle.Bold,
				alignment = TextAnchor.MiddleLeft,
				richText = true
			};

			public static readonly GUIStyle Button = new( EditorStyles.toolbarButton )
			{
				alignment = TextAnchor.MiddleCenter,
				richText = true,
				stretchHeight = true,
				fixedHeight = 0
			};
		}
	}
}
