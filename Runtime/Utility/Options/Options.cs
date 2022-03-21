using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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

		[Option, Terminal, Property( "application.fps", 250 ), Title( "FPS Cap" ), Group( "Application" )]
		public static int Framerate
		{
			get => Application.targetFrameRate;
			set => Application.targetFrameRate = value;
		}

		[Option, Terminal, Property( "application.vsync", 0 ), Title( "VSync" ), Group( "Application" )]
		public static int Vsync
		{
			get => QualitySettings.vSyncCount;
			set => QualitySettings.vSyncCount = value;
		}

		[Option, Terminal, Property( "application.fullscreen", true ), Title( "Fullscreen" ), Group( "Application" )]
		public static bool Fullscreen
		{
			get => Screen.fullScreen;
			set => Screen.fullScreen = value;
		}

		[Option, Terminal, Property( "application.fullscreen_mode", FullScreenMode.FullScreenWindow ), Title( "Fullscreen Mode" ), Group( "Application" )]
		public static FullScreenMode FullscreenMode
		{
			get => Screen.fullScreenMode;
			set => Screen.fullScreenMode = value;
		}

		[Option, Terminal, Property( "graphics.shadows_resolution", ShadowResolution.VeryHigh ), Title( "Shadow Resolution" ), Group( "Shadows" )]
		public static ShadowResolution ShadowQuality
		{
			get => QualitySettings.shadowResolution;
			set => QualitySettings.shadowResolution = value;
		}

		[Option, Terminal, Property( "graphics.shadows_quality", UnityEngine.ShadowQuality.All ), Title( "Shadow Quality" ), Group( "Shadows" )]
		public static ShadowQuality ShadowType
		{
			get => QualitySettings.shadows;
			set => QualitySettings.shadows = value;
		}

		[Cookie, Terminal, Property( "graphics.texture_quality", 0 ), Group( "Textures" )]
		public static int TextureQuality
		{
			get => QualitySettings.masterTextureLimit;
			set => QualitySettings.masterTextureLimit = value;
		}

		[Cookie, Terminal, Property( "graphics.anisotropic_textures", AnisotropicFiltering.Enable ), Group( "Textures" )]
		public static AnisotropicFiltering AnisotropicTextures
		{
			get => QualitySettings.anisotropicFiltering;
			set => QualitySettings.anisotropicFiltering = value;
		}
	}
}
