using System;
using System.Collections.Generic;
using Espionage.Engine.Editor;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Espionage.Engine.Tools.Editor
{
	[CreateAssetMenu( fileName = "Build Pipeline", menuName = "Espionage.Engine/Build Pipeline" )]
	public class Builder : ScriptableObject
	{
		[field : SerializeField]
		public Book<Game> Game { get; set; }

		public void Compile() { }
	}

	[CustomEditor( typeof( Builder ) )]
	public class BuilderEditor : UnityEditor.Editor
	{
		public override bool UseDefaultMargins() => false;

		public override VisualElement CreateInspectorGUI()
		{
			var root = new VisualElement();	
			
			root.Add( new HeaderBar( "Project Builder", "Builds the project to the target platform.", null, "Header-Bottom-Border" ) );

			return root;
		}
	}
}
