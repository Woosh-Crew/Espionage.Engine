using UnityEngine;

namespace Espionage.Engine
{
	public interface IControls
	{
		void Build( ref Setup setup );

		public struct Setup
		{
			public Vector2 MouseDelta { get; set; }
			public Quaternion ViewAngles { get; set; }
			public float Forward { get; set; }
			public float Horizontal { get; set; }

			public void Clear()
			{
				MouseDelta = Vector2.zero;
				ViewAngles = Quaternion.identity;

				Forward = 0;
				Horizontal = 0;
			}
		}
	}
}
