using UnityEditor;
using UnityEngine;

namespace Espionage.Engine.Tools.Editor
{
	[InitializeOnLoad]
	public static class HierarchyExtensions
	{
		static HierarchyExtensions()
		{
			EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyItem;
			EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyItem;
		}

		private static void OnHierarchyItem( int instanceID, Rect selectionRect )
		{

		}
	}
}
