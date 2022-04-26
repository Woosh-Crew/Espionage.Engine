using UnityEngine;

namespace Espionage.Engine
{
	public class Controls : Module
	{
		public static Setup Active { get; internal set; }
		public static Mouse Mouse => Active.Mouse;
		public static Cursor Cursor => Active.Cursor;
		public static Scheme Scheme => Active.Scheme;

		// Client

		private Setup _setup = new() { Cursor = new() { Locked = true, Visible = false } };

		protected override void OnReady()
		{
			Local.Client.Input = _setup;
			_setup.Scheme = Engine.Project.SetupControls();
		}

		protected override void OnUpdate()
		{
			// Setup ViewAngles,
			var mouse = new Vector2( Input.GetAxis( "Mouse X" ), Input.GetAxis( "Mouse Y" ) ) * Options.MouseSensitivity;

			_setup.Mouse = new() { Delta = mouse, Wheel = Input.GetAxisRaw( "Mouse ScrollWheel" ) };
			_setup.Forward = Input.GetAxisRaw( "Vertical" );
			_setup.Horizontal = Input.GetAxisRaw( "Horizontal" );

			// Sample Scheme
			foreach ( var binding in _setup.Scheme )
			{
				binding.Sample();
			}

			// Building
			_setup = Engine.Project.BuildControls( _setup );

			// Applying
			UnityEngine.Cursor.visible = _setup.Cursor.Visible;
			UnityEngine.Cursor.lockState = _setup.Cursor.Locked ? CursorLockMode.Locked : CursorLockMode.None;

			if ( !_setup.Cursor.Locked && _setup.Cursor.Confined )
			{
				UnityEngine.Cursor.lockState = CursorLockMode.Confined;
			}
		}

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

				foreach ( var binding in Scheme )
				{
					binding.Clear();
				}

				Forward = 0;
				Horizontal = 0;
			}
		}
	}
}
