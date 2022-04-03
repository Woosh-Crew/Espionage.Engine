using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// <para>
	/// Espionage.Engine's Input System. You should be using
	/// this over UnityEngine.Input.
	/// </para>
	/// <para>
	/// All Controls is , is just a wrapper for Unity's default
	/// Input Manager. Later down the line I'd love to have our
	/// own input readers / streams.
	/// </para>
	/// </summary>
	public static class Controls
	{
		public static Mouse Mouse => _active.Mouse;
		public static Cursor Cursor => _active.Cursor;
		public static Scheme Scheme => _active.Scheme;

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
			/// <summary> Mouse Data, gets reset every Input Sample </summary>
			public Mouse Mouse { get; set; }

			/// <summary> Used to control Cursor Lock and Visibility. </summary>
			public Cursor Cursor { get; set; }

			/// <summary> You're Games keybindings </summary>
			public Scheme Scheme { get; set; }

			public float Forward { get; internal set; }
			public float Horizontal { get; internal set; }

			/// <summary> Where a pawns Eyes should be facing (Angles) </summary>
			public Vector3 ViewAngles { get; set; }

			/// <summary> Clears the Input Setup </summary>
			public void Clear()
			{
				Mouse = default;

				foreach ( var binding in Scheme.Values )
				{
					binding.Clear();
				}

				Forward = 0;
				Horizontal = 0;
			}
		}
	}
}
