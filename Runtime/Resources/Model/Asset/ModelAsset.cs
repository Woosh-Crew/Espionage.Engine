using UnityEngine;

namespace Espionage.Engine.Resources
{
	[CreateAssetMenu( menuName = "Espionage.Engine/Model", fileName = "Model" ), Group( "Models" ), File( Extension = "umdl" )]
	public class ModelAsset : Asset
	{
		public GameObject model;
	}
}
