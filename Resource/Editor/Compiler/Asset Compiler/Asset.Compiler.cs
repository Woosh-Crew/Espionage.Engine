using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Espionage.Engine.Tools.Editor;
using UnityEditor;

namespace Espionage.Engine.Resources.Editor
{
	[CustomEditor( typeof( Asset ), true )]
	public class AssetCompiler : UnityEditor.Editor
	{
		private Asset Asset => target as Asset;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			using ( _ = new EditorGUI.DisabledScope( !Asset.CanCompile() ) )
			{
				if ( GUILayout.Button( "Compile" ) )
				{
					Asset.Compile();
				}
			}
		}
	}
}
