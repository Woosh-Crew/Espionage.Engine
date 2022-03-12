using UnityEngine;

namespace Espionage.Engine
{
	[Library]
	public static class Options
	{
		[ConVar, Property( "application.fps" ), Group( "Graphics" ), Title( "Framerate Cap" ), PrefVar]
		private static int Framerate
		{
			get => Application.targetFrameRate;
			set => Application.targetFrameRate = value;
		}

		[ConVar, Property( "application.vsync" ), Group( "Graphics" ), Title( "VSync" ), PrefVar]
		private static int Vsync
		{
			get => QualitySettings.vSyncCount;
			set => QualitySettings.vSyncCount = value;
		}

	}
}
