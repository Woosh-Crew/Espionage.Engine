using Espionage.Engine.Internal;
using UnityEditor;
using UnityEngine;

namespace Espionage.Engine.Editor
{
	[CustomEditor( typeof( Entity ), true )]
	public class EntityEditor : UnityEditor.Editor
	{
		private Library ClassInfo { get; set; }

		private void OnEnable()
		{
			ClassInfo = Library.Database[target.GetType()];
			EditorInjection.Titles[target.GetType()] = $"{ClassInfo.Title} (Entity)";
		}

		public override void OnInspectorGUI()
		{
			if ( ClassInfo.Components.Get<EditableAttribute>()?.Editable ?? true )
			{
				DrawPropertiesExcluding( serializedObject, "m_Script" );
				serializedObject.ApplyModifiedProperties();
			}
			else
			{
				GUILayout.Label( "Not Editable", EditorStyles.miniBoldLabel );
			}
		}
	}
}
