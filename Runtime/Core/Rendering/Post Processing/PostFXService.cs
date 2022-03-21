using System;
using Espionage.Engine.Services;
using ImGuiNET;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Object = UnityEngine.Object;

namespace Espionage.Engine.PostProcessing
{
	[Title( "Post Processing" )]
	public class PostFXService : Service
	{
		private PostProcessDebug _debug;
		private PostProcessLayer _layer;

		public override void OnReady()
		{
			// Get the Main Camera
			var camera = Engine.Services.Get<CameraService>().Camera;

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
					Item( item.ToString(), item );
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

		// AO

		[Option, Terminal, Property( "postfx.enable_ao", true ), Title( "Enable Ambient Occlusion" ), Group( "Post Processing" )]
		public static bool EnableAO { get; set; }

		// Bloom

		[Option, Terminal, Property( "postfx.enable_bloom", true ), Title( "Enable Bloom" ), Group( "Post Processing" )]
		public static bool EnableBloom { get; set; }

		[Option, Terminal, Property( "postfx.fast_bloom", false ), Title( "Fast Bloom" ), Group( "Post Processing" )]
		public static bool FastBloom { get; set; }

		// SSR

		[Option, Terminal, Property( "postfx.enable_ssr", true ), Title( "Enable Screen Space Reflections" ), Group( "Post Processing" )]
		public static bool EnableSSR { get; set; }

		[Option, Terminal, Property( "postfx.ssr_quality", ScreenSpaceReflectionPreset.Medium ), Title( "Screen Space Reflections Quality" ), Group( "Post Processing" )]
		public static ScreenSpaceReflectionPreset QualitySSR { get; set; }

		// Motion Blur

		[Option, Terminal, Property( "postfx.enable_motion_blur", false ), Title( "Enable Motion Blur" ), Group( "Post Processing" )]
		public static bool EnableMotionBlur { get; set; }

		// Depth of Field

		[Option, Terminal, Property( "postfx.enable_dof", false ), Title( "Enable Depth of Field" ), Group( "Post Processing" )]
		public static bool EnableDOF { get; set; }

		// Chromatic Aberration

		[Option, Terminal, Property( "postfx.enable_ca", true ), Title( "Enable Chromatic Aberration" ), Group( "Post Processing" )]
		public static bool EnableChromaticAberration { get; set; }

		[Option, Terminal, Property( "postfx.fast_ca", false ), Title( "Fast Chromatic Aberration" ), Group( "Post Processing" )]
		public static bool FastChromaticAberration { get; set; }


		// Assigning

		[Function( "postfx.apply" ), Terminal, Callback( "map.loaded" ), Callback( "cookies.saved" )]
		private static void SetPostFX()
		{
			// We don't need to set shit if its quiting
			if ( Engine.IsQuitting )
			{
				return;
			}

			// Get all Post FX.
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
