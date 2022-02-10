using UnityEditor;
using UnityEngine;

namespace Espionage.Engine.Editor.Drawers
{
	[CustomPropertyDrawer( typeof( Library ) )]
	public class LibraryDrawer : PropertyDrawer
	{
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			GUI.Label( position, "Can't use library in Editor, use LibraryRef instead" );
		}
	}
}
