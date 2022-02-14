using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Espionage.Engine.Resources
{
	/// <summary>
	/// A reference to a scene. This is used for compiling maps.
	/// </summary>
	[CreateAssetMenu( fileName = "Map", menuName = "Map", order = 0 ), File( Extension = "map" ), Group( "Maps" )]
	public sealed class MapReference : Asset
	{
		public string title;
		public string description;
		public Texture2D icon;

		//
		// Editor Only
		//

	#if UNITY_EDITOR

		public SceneAsset sceneAsset;
		public BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.ChunkBasedCompression;

		public override bool CanCompile()
		{
			return sceneAsset != null;
		}


	#endif
	}
}
