using System;
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
			EditorInjection.Titles[target.GetType()] = $"{ClassInfo.Title}";

			// Only Entities can have custom icons...
			if ( ClassInfo.Components.TryGet<IconAttribute>( out var icon ) )
			{
				EditorGUIUtility.SetIconForObject( target, icon.Icon );
			}
		}

		private void OnSceneGUI() { }

		public override void OnInspectorGUI()
		{
			var rect = EditorGUILayout.GetControlRect( true, 24, GUIStyle.none );
			rect.width = Screen.width;
			rect.y = 0;
			rect.x = 0;

			// Box
			EditorGUI.DrawRect( rect, new Color32( 45, 45, 45, 255 ) );

			// Label
			rect.x = 16;
			rect.width -= 16;
			GUI.Label( rect, $"Entity  <size=10>[{ClassInfo.Name} / {ClassInfo.Group}]</size>", Styles.Text );

			// Underline
			rect.y += 24;
			rect.height = 1;
			rect.width += 16;
			rect.x = 0;
			EditorGUI.DrawRect( rect, new Color32( 25, 25, 25, 255 ) );


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
		}
	}
}
