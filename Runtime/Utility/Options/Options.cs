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

		[Option, Terminal, Property( "application.fullscreen", true ), Title( "Fullscreen" ), Group( "Graphics" )]
		public static bool Fullscreen
		{
			get => Screen.fullScreen;
			set => Screen.fullScreen = value;
		}

		[Option, Terminal, Property( "application.fullscreen_mode", FullScreenMode.FullScreenWindow ), Title( "Fullscreen Mode" ), Group( "Graphics" )]
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

		[Option, Terminal, Property( "graphics.enable_ao", true ), Title( "Enable AO" ), Group( "Graphics" )]
		public static bool EnableAO { get; set; }

		[Option, Terminal, Property( "graphics.ao_quality", AmbientOcclusionQuality.Medium ), Title( "AO Quality" ), Group( "Graphics" )]
		public static AmbientOcclusionQuality QualityAO { get; set; }

		// Bloom

		[Option, Terminal, Property( "graphics.enable_bloom", true ), Title( "Enable Bloom" ), Group( "Graphics" )]
		public static bool EnableBloom { get; set; }

		[Option, Terminal, Property( "graphics.bloom_quality", false ), Title( "Bloom Quality" ), Group( "Graphics" )]
		public static bool FastBloom { get; set; }

		// Assigning

		[Function, Callback( "map.loaded" ), Callback( "cookies.saved" )]
		private static void SetPostFX()
		{
			// We don't need to set shit if its quiting
			if ( Engine.IsQuitting )
			{
				return;
			}

			// Get all Post FX.
			Debugging.Log.Info( "Changing PostFX" );

			var all = Object.FindObjectsOfType<PostProcessVolume>();

			foreach ( var volume in all )
			{
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
					ao.mode.Override( AmbientOcclusionMode.ScalableAmbientObscurance );
					ao.quality.Override( QualityAO );
				}
			}

			// Bloom
			if ( profile.HasSettings<Bloom>() )
			{
				var bloom = profile.GetSetting<Bloom>();
				bloom.active = EnableAO;

				// Do shit only if we're active
				if ( bloom.active )
				{
					bloom.fastMode.Override( FastBloom );
				}
			}
		}
	}
}
