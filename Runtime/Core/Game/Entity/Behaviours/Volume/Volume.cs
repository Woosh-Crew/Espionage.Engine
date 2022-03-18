using UnityEngine;

namespace Espionage.Engine.Volumes
{
	public class Volume : Entity
	{
		public interface ICallbacks { }
		
		// Fields

		[SerializeField]
		private float blendDistance;

		[SerializeField]
		private int priority;
	}
}
