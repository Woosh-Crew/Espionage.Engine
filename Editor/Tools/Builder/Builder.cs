﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine.Tools.Editor
{
	[CreateAssetMenu]
	public class Builder : ScriptableObject
	{
		[field : SerializeField]
		public List<Action> Actions { get; set; }

		public class Action : ScriptableObject { }

#if UNITY_EDITOR

		public void Build( BuildTarget target, BuildOptions options )
		{
			if ( Engine.Game == null )
			{
				Dev.Log.Error( "No Game!" );
				return;
			}

			var info = Engine.Game.ClassInfo;

			PlayerSettings.productName = info.Title;
			PlayerSettings.bundleVersion = version.ToString();

			// Setup BuildPipeline
			var buildSettings = new BuildPlayerOptions()
			{
				locationPathName = $"Exports/{info.Title} v{version}/{info.Name}.exe",
				options = options,
				target = target,
				targetGroup = BuildTargetGroup.Standalone
			};

			Callback.Run( "project_builder.building", target, buildSettings );
			var report = UnityEditor.BuildPipeline.BuildPlayer( buildSettings );

			if ( report != null )
			{
				version++;
			}
		}

		public void Play( string launchArgs = null )
		{
			var info = Engine.Game.ClassInfo;
			Process.Start( Files.Pathing.Absolute( $"Exports/{info.Title} v{version - 1}/{info.Name}.exe" ), launchArgs );
		}

#endif

		// Fields

		[SerializeField]
		private bool master;

		[SerializeField]
		private int version;
	}

	#if UNITY_EDITOR

	[CustomEditor( typeof( Builder ) )]
	public class BuilderEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if ( GUILayout.Button( "Build" ) )
			{
				((Builder)target).Build( BuildTarget.StandaloneWindows64, BuildOptions.None );
			}

			if ( GUILayout.Button( "Test" ) )
			{
				((Builder)target).Play();
			}
		}
	}

	#endif
}