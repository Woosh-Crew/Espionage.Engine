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
			public Vector2 MouseDelta;

			/// <summary> Where a pawns Eyes should be facing (Angles) </summary>
			public Vector3 ViewAngles;

			/// <summary> Forward Direction </summary>
			public float Forward;

			/// <summary> Horizontal Direction </summary>
			public float Horizontal;

			/// <summary> Clears the Input Setup </summary>
			public void Clear()
			{
				MouseDelta = Vector2.zero;

				Forward = 0;
				Horizontal = 0;

				Input.ResetInputAxes();
			}
		}
	}
}
