using System;
using Espionage.Engine.Internal;
using NUnit.Framework.Internal.Filters;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Espionage.Engine.Editor
{
	[CustomEditor( typeof( Behaviour ), true )]
	public class BehaviourEditor : UnityEditor.Editor
	{
		private Library ClassInfo { get; set; }

		private void OnEnable()
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
			}
			else
			{
				GUILayout.Label( "Not Editable", EditorStyles.miniBoldLabel );
			}
		}
	}

}
