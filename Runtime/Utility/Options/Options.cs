using UnityEngine;

namespace Espionage.Engine
{
	[Library, Group( "Preferences" )]
	public static class Options
	{
		[Option, ConVar, Property( "application.fps", 250 ), Title( "FPS Cap" ), Group( "Graphics" )]
		public static int Framerate
		{
			get => Application.targetFrameRate;
			set => Application.targetFrameRate = value;
		}

		[Option, ConVar, Property( "application.vsync", 0 ), Title( "VSync" ), Group( "Graphics" )]
		public static int Vsync
		{
			get => QualitySettings.vSyncCount;
			set => QualitySettings.vSyncCount = value;
		}
	}
}
