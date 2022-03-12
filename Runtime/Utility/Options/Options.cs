using UnityEngine;

namespace Espionage.Engine
{
	[Library, Group( "Preferences" )]
	public static class Options
	{
		// Default Options

		// Controls

		[Option, Terminal, Property( "controls.ms_sensitivity", 1.5f ), Title( "Mouse Sensitivity" ), Group( "Controls" )]
		public static float MouseSensitivity { get; set; }

		// Graphics

		[Option, Terminal, Property( "application.fps", 250 ), Title( "FPS Cap" ), Group( "Graphics" )]
		public static int Framerate
		{
			get => Application.targetFrameRate;
			set => Application.targetFrameRate = value;
		}

		[Option, Terminal, Property( "application.vsync", 0 ), Title( "VSync" ), Group( "Graphics" )]
		public static int Vsync
		{
			get => QualitySettings.vSyncCount;
			set => QualitySettings.vSyncCount = value;
		}
	}
}
