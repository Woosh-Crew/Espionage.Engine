using System;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Espionage.Engine's Input System. You should be using
	/// this over UnityEngine.Input.
	/// </summary>
	public static class Controls
	{
		public static Mouse Mouse => _active.Mouse;
		public static Cursor Cursor => _active.Cursor;
		public static Scheme Scheme { get; }

		internal static void SetSetup( Client client )
		{
			_active = client.Input;
		}

		// Fields

		private static Setup _active;

		/// <summary>
		/// Controls the raw values of Input. This is what would be built
		/// and sent to the server when networking is implemented. 
		/// </summary>
		public class Setup
		{
			public Mouse Mouse { get; set; }
			public Cursor Cursor { get; set; }

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
	}

	public class Scheme : Dictionary<string, Binding>
	{
		public Scheme() : base( StringComparer.CurrentCultureIgnoreCase ) { }
	}

	/// <summary>
	/// Struct containing data about the mouse
	/// </summary>
	public struct Mouse
	{
		public Vector2 Delta { get; internal set; }
		public float Wheel { get; internal set; }
	}

	/// <summary>
	/// Struct containing data about the cursor
	/// </summary>
	public class Cursor
	{
		public bool Visible { get; set; }
		public bool Locked { get; set; }
		public bool Confined { get; set; }
	}


	public struct Binding
	{
		public KeyCode Key { get; }

		public bool Pressed => Input.GetKeyDown( Key );
		public bool Down => Input.GetKey( Key );
		public bool Released => Input.GetKeyUp( Key );
	}
}
