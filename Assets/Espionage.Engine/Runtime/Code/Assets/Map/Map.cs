using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine
{
	[CreateAssetMenu( menuName = "Espionage.Engine/Asset/Map", fileName = "Map" )]
	[Library( "asset.map", Title = "Map" )]
	public class Map : Asset
	{
		public string title;
		public SceneAsset scene;

#if UNITY_EDITOR

		public override void Compile()
		{
			var path = AssetDatabase.GetAssetPath( this );
		}

#endif
	}
}
