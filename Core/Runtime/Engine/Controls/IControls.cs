
using UnityEngine;

namespace Espionage.Engine
{
	public interface IControls
	{
		void Build( ref Setup setup );
		
		public struct Setup
		{
			public Vector2 ViewAngles { get; set; }
			public float Forward { get; set; }
			public float Horizontal { get; set; }
		}
	}
}
