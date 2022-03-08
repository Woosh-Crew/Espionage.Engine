using Espionage.Engine.Internal;
using UnityEditor;
using UnityEngine;

namespace Espionage.Engine.Editor
{
	[CustomEditor( typeof( Component ), true )]
	public class ComponentEditor : UnityEditor.Editor
	{
		private Library ClassInfo { get; set; }

		protected virtual void OnEnable()
		{
			ClassInfo = Library.Database[target.GetType()];
			EditorInjection.Titles[target.GetType()] = $"{ClassInfo.Title} (Component)";
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
