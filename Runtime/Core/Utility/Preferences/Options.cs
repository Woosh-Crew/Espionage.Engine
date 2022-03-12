using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Espionage.Engine
{
	[Library]
	public static class Options
	{
		[ConVar, Property( "application.fps" ), Group( "Graphics" ), Title( "Framerate Cap" ), PrefVar]
		public static int Framerate
		{
			get => Application.targetFrameRate;
			set => Application.targetFrameRate = value;
		}

		[ConVar, Property( "application.vsync" ), Group( "Graphics" ), Title( "VSync" ), PrefVar]
		public static int Vsync
		{
			get => QualitySettings.vSyncCount;
			set => QualitySettings.vSyncCount = value;
		}

		//
		// Post Processing
		//

		// Ambient Occlusion

		[ConVar, Property( "graphics.ao" ), Group( "Graphics/Ambient Occlusion" ), Title( "Enable Ambient Occlusion" ), PrefVar]
		public static bool EnableAO { get; set; }

		[ConVar, Property( "graphics.ao_quality" ), Group( "Graphics/Ambient Occlusion" ), Title( "Ambient Occlusion Quality" ), PrefVar]
		public static AmbientOcclusionQuality QualityAO { get; set; }

		// Bloom

		[ConVar, Property( "graphics.bloom" ), Group( "Graphics/Bloom" ), Title( "Enable Bloom" ), PrefVar]
		public static bool EnableBloom { get; set; }

		[ConVar, Property( "graphics.bloom_quality" ), Group( "Graphics/Bloom" ), Title( "Fast Bloom" ), PrefVar]
		public static bool FastBloom { get; set; }

		// Screen Space Reflections

		[ConVar, Property( "graphics.ssr" ), Group( "Graphics/Screen Space Reflections" ), Title( "Enable SSR" ), PrefVar]
		public static bool EnableSSR { get; set; }

		[ConVar, Property( "graphics.ssr_quality" ), Group( "Graphics/Screen Space Reflections" ), Title( "SRR Quality" ), PrefVar]
		public static ScreenSpaceReflectionPreset QualitySSR { get; set; }

		// Chromatic Aberration

		[ConVar, Property( "graphics.ca" ), Group( "Graphics/Chromatic Aberration" ), Title( "Enable Chromatic Aberration" ), PrefVar]
		public static bool EnableCA { get; set; }

		[ConVar, Property( "graphics.ca_quality" ), Group( "Graphics/Chromatic Aberration" ), Title( "CA Quality" ), PrefVar]
		public static bool FastCA { get; set; }

		// Motion Blur

		[ConVar, Property( "graphics.motion_blur" ), Group( "Graphics/Motion Blur" ), Title( "Enable Motion Blur" ), PrefVar]
		public static bool EnableMotionBlur { get; set; }

		// Depth of Field

		[ConVar, Property( "graphics.dof" ), Group( "Graphics/Depth of Field" ), Title( "Enable Depth of Field" ), PrefVar]
		public static bool EnableDOF { get; set; }
	}
}
