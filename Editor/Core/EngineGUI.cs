using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Espionage.Engine.Editor
{
	public class EngineGUI
	{
		public static void Line()
		{
			EditorGUILayout.LabelField( "", GUI.skin.horizontalSlider );
		}

		public static bool Foldout( GUIContent header, ref bool state, float height = 26 )
		{
			// Set the icon size to match height
			EditorGUIUtility.SetIconSize( new( 20, 20 ) );

			// Create GUIStyle for Little Arrows
			var arrowStyle = new GUIStyle( "Label" )
			{
				alignment = TextAnchor.MiddleLeft,
				fontSize = (int)height - 16,
				fontStyle = FontStyle.Bold,
				padding = new( 16, 0, 0, 0 )
			};

			// Draw actual button
			if ( GUILayout.Button( header, Styles.FoldoutStyle, GUILayout.ExpandWidth( true ), GUILayout.Height( height ) ) ) { state = !state; }

			GUI.Label( GUILayoutUtility.GetLastRect(), state ? "▼" : "▶", arrowStyle );

			GUILayout.Space( 8 );
			EditorGUIUtility.SetIconSize( new( 0, 0 ) );

			// Return the current state
			return state;
		}

		public static void Header( AdvancedDropdown dropdown, Texture icon, SerializedProperty name, SerializedProperty className, SerializedProperty disabled )
		{
			if ( icon != null )
			{
				GUILayout.Space( 8 );
				EditorGUIUtility.SetIconSize( new( 48, 48 ) );
				GUILayout.Label( icon, GUILayout.Width( 64 ), GUILayout.Height( 48 ) );
				EditorGUIUtility.SetIconSize( new( 0, 0 ) );
			}

			GUILayout.BeginVertical( GUILayout.ExpandWidth( true ) );
			{
				EditorGUI.BeginChangeCheck();
				name.stringValue = GUILayout.TextField( name.stringValue, Styles.TextField, GUILayout.Height( 26 ) );

				// Remove Spaces from entity name
				if ( EditorGUI.EndChangeCheck() )
				{
					name.stringValue = name.stringValue.ToProgrammerCase();
				}

				if ( string.IsNullOrEmpty( name.stringValue ) )
				{
					GUI.Label( GUILayoutUtility.GetLastRect(), " <color=grey>Entity Name</color>", Styles.TextFieldGhost );
				}

				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					var rect = GUILayoutUtility.GetRect( new( className.stringValue ), EditorStyles.popup );
					if ( GUI.Button( rect, new GUIContent( className.stringValue ), EditorStyles.popup ) )
					{
						dropdown.Show( rect );
					}

					GUILayout.BeginHorizontal( GUILayout.Width( 64 ) );
					{
						GUILayout.Label( "Start Disabled:" );
						disabled.boolValue = EditorGUILayout.Toggle( disabled.boolValue );
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}

		public static class Styles
		{
			public static readonly GUIStyle FoldoutStyle = new( EditorStyles.miniButtonMid )
			{
				richText = true,
				alignment = TextAnchor.MiddleLeft,
				fontSize = 16,
				fontStyle = FontStyle.Bold,
				padding = new( EditorStyles.inspectorDefaultMargins.padding.left + 16, 0, 0, 0 ),
				overflow = new( 0, 0, 0, 0 ),
				border = new( 0, 0, 0, 0 ),
				margin = new( 2, 0, 0, 0 ),
				imagePosition = ImagePosition.ImageLeft,
				fixedHeight = 0
			};

			public static readonly GUIStyle HeaderStyle = new( "window" )
			{
				richText = true,
				alignment = TextAnchor.UpperLeft,
				fontSize = 16,
				fontStyle = FontStyle.Bold,
				padding = new( 8, 8, 8, 8 ),
				margin = new( 8, 8, 8, 0 )
			};

			public static readonly GUIStyle TextField = new( EditorStyles.textField )
			{
				richText = true,
				alignment = TextAnchor.MiddleLeft,
				padding = new( 6, 0, 0, 0 ),
				fontSize = 12,
				fontStyle = FontStyle.Bold
			};

			public static readonly GUIStyle TextFieldGhost = new( "Label" )
			{
				richText = true,
				alignment = TextAnchor.MiddleLeft,
				padding = new RectOffset( 6, 0, 0, 0 ),
				fontSize = 12,
				fontStyle = FontStyle.Bold
			};
		}
	}
}
