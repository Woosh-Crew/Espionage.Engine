using UnityEngine;

namespace Espionage.Engine
{
	public interface IControls
	{
		void Build( ref Setup setup );

		/// <summary> Controls the raw values of Input. </summary>
		public struct Setup
		{
			/// <summary> The Current Mouse Delta </summary>
			public Vector2 MouseDelta { get; set; }

			/// <summary> Where a pawns Eyes should be facing (Angles) </summary>
			public Vector3 ViewAngles { get; set; }

			/// <summary> Forward Direction </summary>
			public float Forward { get; set; }

			/// <summary> Horizontal Direction </summary>
			public float Horizontal { get; set; }

			/// <summary> Clears the Input Setup </summary>
			public void Clear()
			{
				MouseDelta = Vector2.zero;
				ViewAngles = Vector3.zero;

				Forward = 0;
				Horizontal = 0;
			}
		}
	}
}
