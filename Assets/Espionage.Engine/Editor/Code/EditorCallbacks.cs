using UnityEditor;

namespace Espionage.Engine.Internal.Editor
{
	// Run all the callbacks related to Unity
	internal static class EditorCallbacks
	{
		[InitializeOnLoadMethod]
		public static void Initialize()
		{
			SceneView.beforeSceneGui += ( view ) => Callback.Run( "editor.scene.draw", true, view );
		}
	}
}
