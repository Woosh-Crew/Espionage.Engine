using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Espionage.Engine.Tools.Editor;
using UnityEditor;

namespace Espionage.Engine.Resources.Editor
{
	[CustomEditor( typeof( Asset<> ), true )]
	public class AssetCompiler : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if ( GUILayout.Button( "Compile" ) )
			{
				Debugging.Log.Info( "Compile!" );
			}
		}
	}
}
