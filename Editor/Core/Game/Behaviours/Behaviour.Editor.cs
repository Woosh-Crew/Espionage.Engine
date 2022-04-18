using Espionage.Engine.Internal;
using UnityEditor;
using UnityEngine;

namespace Espionage.Engine.Editor
{
	[CustomEditor( typeof( Behaviour ), true )]
	public class BehaviourEditor : UnityEditor.Editor
	{
		public Library ClassInfo { get; set; }

		protected virtual void OnEnable()
		{
			ClassInfo = Library.Database[target.GetType()];
			EditorInjection.Titles[target.GetType()] = $"{ClassInfo.Title} (Behaviour)";
		}

		public override void OnInspectorGUI()
		{
			if ( ClassInfo.Components.Get<EditableAttribute>()?.Editable ?? true )
			{
				DrawPropertiesExcluding( serializedObject, "m_Script" );
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
	}

}
