using UnityEditor;
using UnityEngine;

namespace Espionage.Engine.Internal.Editor
{
	public class Toolbar : EditorWindow
	{
		[MenuItem( "Espionage.Engine/Toolbar" )]
		private static void OpenToolbar()
		{
			var window = EditorWindow.GetWindow<Toolbar>();
			window.titleContent.text = "Toolbar";

			var height = 64;

			window.maxSize = new Vector2( 6000000, height );
			window.minSize = new Vector2( 32, height );
			window.Show();
		}
	}
}
