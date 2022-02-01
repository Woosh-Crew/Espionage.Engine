using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Espionage.Engine.Editor
{
	public static partial class EditorCallback
	{
		/// <summary>
		/// Anything having to do with <see cref="UnityEditor.EditorApplication"/>
		/// </summary>
		public static class Application
		{
			/// <summary>
			/// Required Callback Arguments: 
			/// <para>New State: <see cref="UnityEditor.PlayModeStateChange"/></para>
			/// </summary>
			public class PlayModeChanged : CallbackAttribute
			{
				public const string Identifier = "editor.playmode.changed";
				public PlayModeChanged() : base( Identifier ) { }
			}
		}

		/// <summary>
		/// Anything having to do with the <see cref="UnityEditor.SceneManagement.EditorSceneManager"/>
		/// </summary>
		public static class SceneManager
		{
			//
			// Utility
			//

			/// <summary>
			/// Required Callback Arguments: 
			/// <para>Scene: <see cref="UnityEngine.SceneManagement.Scene"/></para>
			/// <para>Setup: <see cref="UnityEditor.SceneManagement.NewSceneSetup"/></para>
			/// <para>Mode: <see cref="UnityEditor.SceneManagement.NewSceneMode"/></para>
			/// </summary>
			public class Created : CallbackAttribute
			{
				public const string Identifier = "editor.scene.new_scene";
				public Created() : base( Identifier ) { }
			}

			/// <summary>
			/// Required Callback Arguments: 
			/// <para>Scene: <see cref="UnityEngine.SceneManagement.Scene"/></para>
			/// </summary>
			public class Dirtied : CallbackAttribute
			{
				public const string Identifier = "editor.scene.dirtied";
				public Dirtied() : base( Identifier ) { }
			}

			//
			// Closing
			//

			/// <summary>
			/// Required Callback Arguments: 
			/// <para>Scene: <see cref="UnityEngine.SceneManagement.Scene"/></para>
			/// </summary>
			public class Closed : CallbackAttribute
			{
				public const string Identifier = "editor.scene.closed";
				public Closed() : base( Identifier ) { }
			}

			/// <summary>
			/// Required Callback Arguments: 
			/// <para>Scene: <see cref="UnityEngine.SceneManagement.Scene"/></para>
			/// <para>Removing Scene: <see cref="System.Boolean"/></para>
			/// </summary>
			public class Closing : CallbackAttribute
			{
				public const string Identifier = "editor.scene.closing";
				public Closing() : base( Identifier ) { }
			}

			//
			// Opening
			//

			/// <summary>
			/// Required Callback Arguments: 
			/// <para>Path: <see cref="System.String"/></para>
			/// <para>Mode: <see cref="UnityEditor.SceneManagement.OpenSceneMode"/></para>
			/// </summary>
			public class Opening : CallbackAttribute
			{
				public const string Identifier = "editor.scene.opening";
				public Opening() : base( Identifier ) { }
			}

			/// <summary>
			/// Required Callback Arguments: 
			/// <para>Scene: <see cref="UnityEngine.SceneManagement.Scene"/></para>
			/// <para>Mode: <see cref="UnityEditor.SceneManagement.OpenSceneMode"/></para>
			/// </summary>
			public class Opened : CallbackAttribute
			{
				public const string Identifier = "editor.scene.opened";
				public Opened() : base( Identifier ) { }
			}

			// 
			// Saving
			//

			/// <summary>
			/// Required Callback Arguments: 
			/// <para>Scene: <see cref="UnityEngine.SceneManagement.Scene"/></para>
			/// <para>Path: <see cref="System.String"/></para>
			/// </summary>
			public class Saving : CallbackAttribute
			{
				public const string Identifier = "editor.scene.saving";
				public Saving() : base( Identifier ) { }
			}

			/// <summary>
			/// Required Callback Arguments: 
			/// <para>Scene: <see cref="UnityEngine.SceneManagement.Scene"/></para>
			/// </summary>
			public class Saved : CallbackAttribute
			{
				public const string Identifier = "editor.scene.saved";
				public Saved() : base( Identifier ) { }
			}
		}

		/// <summary>
		/// Anything having to do with the <see cref="UnityEditor.SceneView"/>
		/// </summary>
		public static class SceneView
		{
			/// <summary>
			/// Required Callback Arguments: 
			/// <para>SceneView: <see cref="UnityEditor.SceneView"/></para>
			/// </summary>
			public class Drawing : CallbackAttribute
			{
				public const string Identifier = "editor.scene_view.draw";
				public Drawing() : base( Identifier ) { }
			}
		}
	}
}
