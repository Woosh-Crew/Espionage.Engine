using System;
using Espionage.Engine.Services;
using ImGuiNET;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Object = UnityEngine.Object;

namespace Espionage.Engine.PostProcessing
{
	[Library( "services.postfx" ), Group( "Post Processing" )]
	public class PostFXService : Service
	{
		private PostProcessDebug _debug;
		private PostProcessLayer _layer;

		public override void OnReady()
		{
			// Get the Main Camera
			var camera = Engine.Camera;

			// Setup Post Processing
			_layer = camera.gameObject.AddComponent<PostProcessLayer>();
			_layer.Init( UnityEngine.Resources.Load<PostProcessResources>( "PostProcessResources" ) );

			_layer.volumeTrigger = camera.transform;
			_layer.volumeLayer = LayerMask.GetMask( "TransparentFX", "Water" );
			_layer.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;

			_debug = camera.gameObject.AddComponent<PostProcessDebug>();
			_debug.postProcessLayer = _layer;
		}

		//
		// UI
		//

		[Function, Callback( "dev.menu_bar.graphics" )]
		private void MenuBar()
		{
			// Post Processing Debug Overlays
			if ( ImGui.BeginMenu( "Fullbright Overlays" ) )
			{
				foreach ( var value in Enum.GetValues( typeof( DebugOverlay ) ) )
				{
					var item = (DebugOverlay)value;
					Item( item.ToString().ToTitleCase().IsEmpty( "Null" ), item );
				}

				ImGui.EndMenu();
			}
		}

		private void Item( string name, DebugOverlay overlay )
		{
			if ( ImGui.MenuItem( name, null, _debug.debugOverlay == overlay ) )
			{
				_debug.debugOverlay = _debug.debugOverlay == overlay ? DebugOverlay.None : overlay;
			}
		}

		//
		// Options
		//

		private const string Output = "config://postfx.ini";

		// AO

		[Option( Output ), Terminal, Property( "postfx.enable_ao", true ), Title( "Enabled" ), Group( "Ambient Occlusion" )]
		public static bool EnableAO { get; set; }

		// Bloom

		[Option( Output ), Terminal, Property( "postfx.enable_bloom", true ), Title( "Enabled" ), Group( "Bloom" )]
		public static bool EnableBloom { get; set; }

		[Option( Output ), Terminal, Property( "postfx.fast_bloom", false ), Title( "Fast" ), Group( "Bloom" )]
		public static bool FastBloom { get; set; }

		// SSR

		[Option( Output ), Terminal, Property( "postfx.enable_ssr", true ), Title( "Enabled" ), Group( "Screen Space Reflections" )]
		public static bool EnableSSR { get; set; }

		[Option( Output ), Terminal, Property( "postfx.ssr_quality", ScreenSpaceReflectionPreset.Medium ), Title( "Quality" ), Group( "Screen Space Reflections" )]
		public static ScreenSpaceReflectionPreset QualitySSR { get; set; }

		// Motion Blur

		[Option( Output ), Terminal, Property( "postfx.enable_motion_blur", false ), Title( "Enabled" ), Group( "Motion Blur" )]
		public static bool EnableMotionBlur { get; set; }

		// Depth of Field

		[Option( Output ), Terminal, Property( "postfx.enable_dof", false ), Title( "Enabled" ), Group( "Depth of Field" )]
		public static bool EnableDOF { get; set; }

		// Chromatic Aberration

		[Option( Output ), Terminal, Property( "postfx.enable_ca", true ), Title( "Enabled" ), Group( "Chromatic Aberration" )]
		public static bool EnableChromaticAberration { get; set; }

		[Option( Output ), Terminal, Property( "postfx.fast_ca", false ), Title( "Fast" ), Group( "Chromatic Aberration" )]
		public static bool FastChromaticAberration { get; set; }


		// Assigning

		[Function( "postfx.apply" ), Terminal, Callback( "map.loaded" ), Callback( "cookies.saved" )]
		private static void SetPostFX()
		{
			if ( Engine.IsQuitting )
			{
				return;
			}

			// Get all Post FX.
			var all = Object.FindObjectsOfType<PostProcessVolume>();
			Debugging.Log.Info( "Adjusting Post FX Profiles" );

			foreach ( var volume in all )
			{
				if ( volume.sharedProfile == null )
				{
					continue;
				}

				AdjustProfile( volume.profile );
			}
		}

		private static void AdjustProfile( PostProcessProfile profile )
		{
			AdjustSetting<AmbientOcclusion>( profile, EnableAO, occlusion =>
			{
				occlusion.ambientOnly.Override( true );
				occlusion.mode.Override( AmbientOcclusionMode.MultiScaleVolumetricObscurance );
			} );

			AdjustSetting<Bloom>( profile, EnableBloom, bloom =>
			{
				bloom.fastMode.Override( FastBloom );
			} );

			AdjustSetting<ScreenSpaceReflections>( profile, EnableSSR, ssr =>
			{
				ssr.preset.Override( QualitySSR );
			} );

			AdjustSetting<ChromaticAberration>( profile, EnableChromaticAberration, ca =>
			{
				ca.fastMode.Override( FastChromaticAberration );
			} );

			AdjustSetting<MotionBlur>( profile, EnableMotionBlur );
			AdjustSetting<DepthOfField>( profile, EnableDOF );
		}

		private static void AdjustSetting<T>( PostProcessProfile profile, bool enabled, Action<T> setup = null ) where T : PostProcessEffectSettings
		{
			if ( !profile.HasSettings<T>() )
			{
				return;
			}

			var setting = profile.GetSetting<T>();
			setting.active = enabled;

			// Do shit only if we're active
			if ( setting.active )
			{
				setup?.Invoke( setting );
			}
		}
	}
}
