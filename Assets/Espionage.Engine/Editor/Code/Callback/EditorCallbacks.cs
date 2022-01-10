using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Espionage.Engine.Editor
{
	// Run all the callbacks related to Unity
	public static partial class EditorCallback
	{
		[InitializeOnLoadMethod]
		private static void Initialize()
		{
			//
			// Editor Application
			//

			EditorApplication.playModeStateChanged += ( state ) => Callback.Run( EditorCallback.Application.PlayModeChanged.Identifier, state );

			//
			// SceneView Callbacks
			//

			UnityEditor.SceneView.beforeSceneGui += ( view ) => Callback.Run( EditorCallback.SceneView.Drawing.Identifier, true, view );
			UnityEditor.SceneView.duringSceneGui += ( view ) => Callback.Run( "editor.scene_view.during_draw", true, view );

			//
			// Scene Manger
			//

			// Utlity
			EditorSceneManager.newSceneCreated += ( scene, setup, mode ) => Callback.Run( EditorCallback.SceneManager.Created.Identifier, scene, setup, mode );
			EditorSceneManager.sceneDirtied += ( scene ) => Callback.Run( EditorCallback.SceneManager.Dirtied.Identifier, scene );

			// Close
			EditorSceneManager.sceneClosing += ( scene, removingScene ) => Callback.Run( EditorCallback.SceneManager.Closing.Identifier, scene, removingScene );
			EditorSceneManager.sceneClosed += ( scene ) => Callback.Run( EditorCallback.SceneManager.Closed.Identifier, scene );

			// Open
			EditorSceneManager.sceneOpening += ( path, mode ) => Callback.Run( EditorCallback.SceneManager.Opening.Identifier, path, mode );
			EditorSceneManager.sceneOpened += ( scene, mode ) => Callback.Run( EditorCallback.SceneManager.Opened.Identifier, scene, mode );

			// Saving
			EditorSceneManager.sceneSaving += ( scene, mode ) => Callback.Run( EditorCallback.SceneManager.Saving.Identifier, scene, mode );
			EditorSceneManager.sceneSaved += ( scene ) => Callback.Run( EditorCallback.SceneManager.Saved.Identifier, scene );
		}
	}
}
