using UnityEngine;

namespace Espionage.Engine
{
	[Library, Group( "Preferences" )]
	public static class Options
	{
		[Property( "application.fps", 300 ), Title( "Framerate Cap" ), Group( "Graphics" ), Cookie]
		public static int Framerate
		{
			get => Application.targetFrameRate;
			set => Application.targetFrameRate = value;
		}

		[Property( "application.vsync", 0 ), Title( "VSync" ), Group( "Graphics" ), Cookie]
		public static int Vsync
		{
			get => QualitySettings.vSyncCount;
			set => QualitySettings.vSyncCount = value;
		}
	}
}
