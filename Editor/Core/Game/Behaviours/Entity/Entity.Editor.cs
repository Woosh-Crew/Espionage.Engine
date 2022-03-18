using Espionage.Engine.Internal;
using UnityEditor;
using UnityEngine;

namespace Espionage.Engine.Editor
{
	[CustomEditor( typeof( Entity ), true )]
	internal class EntityEditor : BehaviourEditor
	{
		protected Entity Entity => target as Entity;

		protected override void OnEnable()
		{
			base.OnEnable();

			// Check if we're a Singleton, if we are, delete this instance if it already exists
			if ( ClassInfo.Components.TryGet<SingletonAttribute>( out _ ) )
			{
				// Find all objects of Type.
				var objects = FindObjectsOfType( ClassInfo.Class );

				foreach ( var o in objects )
				{
					if ( target != o )
					{
						EditorUtility.DisplayDialog(
							$"Singleton ({ClassInfo.Title}) already exists",
							$"Can't create Entity {ClassInfo.Title} onto this GameObject. An instance of it already exists else where in the scene",
							"Okay" );

						DestroyImmediate( target );
						return;
					}
				}
			}

			EditorInjection.Titles[target.GetType()] = $"{ClassInfo.Title}";

			// Only Entities can have custom icons...
			if ( ClassInfo.Components.TryGet<IconAttribute>( out var icon ) )
			{
				EditorGUIUtility.SetIconForObject( target, icon.Icon );
			}
		}

		public override void OnInspectorGUI()
		{
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
			GUI.Label( rect, $"Entity  <size=10>[{ClassInfo.Name} / {ClassInfo.Group}]</size>", Styles.Text );

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

			base.OnInspectorGUI();
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
