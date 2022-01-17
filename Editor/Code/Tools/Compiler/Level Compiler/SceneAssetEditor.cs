using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Espionage.Engine.Editor.Internal
{
	[CustomEditor( typeof( SceneAsset ) )]
	public class SceneAssetEditor : UnityEditor.Editor
	{
		public override VisualElement CreateInspectorGUI()
		{
			var root = new VisualElement();

			root.Add( new Button() { text = "Create Level Asset" } );

			return root;
		}
	}
}
