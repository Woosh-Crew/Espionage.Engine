using System;
using UnityEditor;

namespace Espionage.Engine.Nodes.Editor
{
	public class GraphViewWindow : EditorWindow
	{
		[MenuItem( "Tools/Nodes/Test" )]
		public static void ShowThingy() => GetWindow<GraphViewWindow>();

		private void CreateGUI()
		{
			rootVisualElement.Add(new GraphUI());
		}
	}
}
