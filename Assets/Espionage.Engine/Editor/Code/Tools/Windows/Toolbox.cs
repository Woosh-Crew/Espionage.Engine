using UnityEngine;
using UnityEditor;

namespace Espionage.Engine.Internal.Editor
{
	public class Toolbox : EditorWindow
	{
		[MenuItem( "Tools/Toolbox" )]
		public static void Create()
		{
			var window = EditorWindow.GetWindow<Toolbox>();
			window.titleContent = new GUIContent( "Toolbox", "Open Tools" );
		}
	}
}
