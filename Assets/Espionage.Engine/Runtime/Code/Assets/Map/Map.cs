using UnityEngine;

namespace Espionage.Engine
{
	[CreateAssetMenu( menuName = "Assets/Map", fileName = "Map" )]
	[Library( "asset.map", Title = "Map" )]
	public class Map : Asset
	{
#if UNITY_EDITOR

		public override void Compile()
		{
		}

#endif
	}
}
