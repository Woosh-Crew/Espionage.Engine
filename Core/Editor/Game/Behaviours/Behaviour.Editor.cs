using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Espionage.Engine.Editor.Inspectors
{
	[CustomEditor( typeof( Behaviour ), true )]
	public class BehaviourEditor : UnityEditor.Editor
	{
		private Behaviour Behaviour => target as Behaviour;
		private Library _library;

		private void OnEnable()
		{
			_library = Library.Database[Behaviour.GetType()];
		}

		public override void OnInspectorGUI()
		{
			GUILayout.Label($"{_library.Group} | {_library.Title}", EditorStyles.centeredGreyMiniLabel);
			GUILayout.Space(4);
			
			base.OnInspectorGUI();
		}
	}
}
