using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Espionage.Engine's Input System. You should be using
	/// this over UnityEngine.Input.
	/// </summary>
	public static class Controls
	{
		internal static Setup Active { get; set; }

		public static MouseInfo Mouse => Active.Mouse;
		public static CursorInfo Cursor => Active.Cursor;

		/// <summary>
		/// Controls the raw values of Input. This is what would be built
		/// and sent to the server when networking is implemented. 
		/// </summary>
		public class Setup
		{
			public MouseInfo Mouse { get; set; }
			public CursorInfo Cursor { get; set; }
			
			public float Forward { get; internal set; }
			public float Horizontal { get; internal set; }

			/// <summary> Where a pawns Eyes should be facing (Angles) </summary>
			public Vector3 ViewAngles { get; set; }

			/// <summary> Clears the Input Setup </summary>
			public void Clear()
			{
				Mouse = default;

				Forward = 0;
				Horizontal = 0;
			}
		}

		/// <summary>
		/// Struct containing data about the mouse
		/// </summary>
		public struct MouseInfo
		{
			public Vector2 Delta { get; internal set; }
			public float Wheel { get; internal set; }
		}
		
		/// <summary>
		/// Struct containing data about the cursor
		/// </summary>
		public class CursorInfo
		{
			public bool Visible { get; set; }
			public bool Locked { get; set; }
		}
	}
}
