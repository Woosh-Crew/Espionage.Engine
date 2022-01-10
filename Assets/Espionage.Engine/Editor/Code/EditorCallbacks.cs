using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Espionage.Engine.Internal.Editor
{
	// Run all the callbacks related to Unity
	internal static class EditorCallbacks
	{
		[InitializeOnLoadMethod]
		public static void Initialize()
		{
			//
			// SceneView Callbacks
			//

			SceneView.beforeSceneGui += ( view ) => Callback.Run( "editor.scene_view.draw", true, view );
			SceneView.duringSceneGui += ( view ) => Callback.Run( "editor.scene_view.during_draw", true, view );

			//
			// Scene Manger
			//

			// Utlity
			EditorSceneManager.newSceneCreated += ( scene, setup, mode ) => Callback.Run( "editor.scene.new_scene", scene, setup, mode );
			EditorSceneManager.sceneDirtied += ( scene ) => Callback.Run( "editor.scene.dirtied", scene );

			// Close
			EditorSceneManager.sceneClosing += ( scene, removingScene ) => Callback.Run( "editor.scene.closed", scene, removingScene );
			EditorSceneManager.sceneClosed += ( scene ) => Callback.Run( "editor.scene.closed", scene );

			// Open
			EditorSceneManager.sceneOpening += ( scene, mode ) => Callback.Run( "editor.scene.opening", scene, mode );
			EditorSceneManager.sceneOpened += ( scene, mode ) => Callback.Run( "editor.scene.opened", scene, mode );

			// Saving
			EditorSceneManager.sceneSaving += ( scene, mode ) => Callback.Run( "editor.scene.saving", scene, mode );
			EditorSceneManager.sceneSaved += ( scene ) => Callback.Run( "editor.scene.saved", scene );
		}
	}
}
