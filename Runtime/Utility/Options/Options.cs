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

		// Post Processing

		// AO

		[Option, Terminal, Property( "graphics.enable_ao", true ), Title( "Enable Ambient Occlusion" ), Group( "Graphics" )]
		public static bool EnableAO { get; set; }

		// Bloom

		[Option, Terminal, Property( "graphics.enable_bloom", true ), Title( "Enable Bloom" ), Group( "Graphics" )]
		public static bool EnableBloom { get; set; }

		[Option, Terminal, Property( "graphics.fast_bloom", false ), Title( "Fast Bloom" ), Group( "Graphics" )]
		public static bool FastBloom { get; set; }

		// SSR

		[Option, Terminal, Property( "graphics.enable_ssr", true ), Title( "Enable Screen Space Reflections" ), Group( "Graphics" )]
		public static bool EnableSSR { get; set; }

		[Option, Terminal, Property( "graphics.ssr_quality", ScreenSpaceReflectionPreset.Medium ), Title( "Screen Space Reflections Quality" ), Group( "Graphics" )]
		public static ScreenSpaceReflectionPreset QualitySSR { get; set; }

		// Motion Blur

		[Option, Terminal, Property( "graphics.enable_motion_blur", false ), Title( "Enable Motion Blur" ), Group( "Graphics" )]
		public static bool EnableMotionBlur { get; set; }

		// Depth of Field

		[Option, Terminal, Property( "graphics.enable_dof", false ), Title( "Enable Depth of Field" ), Group( "Graphics" )]
		public static bool EnableDOF { get; set; }

		// Chromatic Aberration

		[Option, Terminal, Property( "graphics.enable_ca", true ), Title( "Enable Chromatic Aberration" ), Group( "Graphics" )]
		public static bool EnableChromaticAberration { get; set; }

		[Option, Terminal, Property( "graphics.fast_ca", false ), Title( "Fast Chromatic Aberration" ), Group( "Graphics" )]
		public static bool FastChromaticAberration { get; set; }


		// Assigning

		[Function( "options.apply_postfx" ), Callback( "map.loaded" ), Callback( "cookies.saved" )]
		private static void SetPostFX()
		{
			// We don't need to set shit if its quiting
			if ( Engine.IsQuitting )
			{
				return;
			}

			// Get all Post FX.
			Dev.Log.Info( "Changing PostFX" );

			var all = Object.FindObjectsOfType<PostProcessVolume>();

			foreach ( var volume in all )
			{
				if ( volume.sharedProfile == null )
				{
					continue;
				}

				AdjustProfile( volume.sharedProfile );
			}
		}

		private static void AdjustProfile( PostProcessProfile profile )
		{
			// AO
			if ( profile.HasSettings<AmbientOcclusion>() )
			{
				var ao = profile.GetSetting<AmbientOcclusion>();
				ao.active = EnableAO;

				// Do shit only if we're active
				if ( ao.active )
				{
					ao.ambientOnly.Override( true );
					ao.mode.Override( AmbientOcclusionMode.MultiScaleVolumetricObscurance );
				}
			}

			// Bloom
			if ( profile.HasSettings<Bloom>() )
			{
				var bloom = profile.GetSetting<Bloom>();
				bloom.active = EnableBloom;

				// Do shit only if we're active
				if ( bloom.active )
				{
					bloom.fastMode.Override( FastBloom );
				}
			}

			// Screen Space Reflections
			if ( profile.HasSettings<ScreenSpaceReflections>() )
			{
				var ssr = profile.GetSetting<ScreenSpaceReflections>();
				ssr.active = EnableSSR;

				// Do shit only if we're active
				if ( ssr.active )
				{
					ssr.preset.Override( QualitySSR );
				}
			}

			// Motion Blur
			if ( profile.HasSettings<MotionBlur>() )
			{
				var blur = profile.GetSetting<MotionBlur>();
				blur.active = EnableMotionBlur;
			}

			// Depth of Field
			if ( profile.HasSettings<DepthOfField>() )
			{
				var dof = profile.GetSetting<DepthOfField>();
				dof.active = EnableDOF;
			}

			// Chromatic Aberration
			if ( profile.HasSettings<ChromaticAberration>() )
			{
				var ca = profile.GetSetting<ChromaticAberration>();
				ca.active = EnableChromaticAberration;

				if ( ca.active )
				{
					ca.fastMode.Override( FastChromaticAberration );
				}
			}
		}
	}
}
