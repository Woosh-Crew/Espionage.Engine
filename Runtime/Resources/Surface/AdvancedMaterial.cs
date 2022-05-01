using Espionage.Engine.Resources;
using UnityEngine;

namespace Espionage.Engine
{
	public class AdvancedMaterial : MonoBehaviour, ISurface
	{
		public Surface Surface => null;

		[SerializeField]
		private string surface;
	}
}
