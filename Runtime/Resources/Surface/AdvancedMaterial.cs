using UnityEngine;

namespace Espionage.Engine
{
	public class AdvancedMaterial : MonoBehaviour, ISurface
	{
		public Surface Surface => surface;
		
		[SerializeField]
		private Surface surface;
	}
}
