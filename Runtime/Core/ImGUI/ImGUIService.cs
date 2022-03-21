using System;
using Espionage.Engine.ImGUI.Assets;
using Espionage.Engine.ImGUI.Events;
using Espionage.Engine.ImGUI.Platform;
using Espionage.Engine.ImGUI.Renderer;
using Espionage.Engine.Services;
using ImGuiNET;
using UnityEngine;
using UnityEngine.Rendering;

namespace Espionage.Engine.ImGUI
{
	[Order( 50 ), Title( "ImGUI" )]
	public sealed class ImGUIService : Service
	{
		private ShaderResourcesAsset Shaders { get; set; }
		private StyleAsset Style { get; set; }
		private CursorShapesAsset Cursor { get; set; }

		private Context _context;
		private IRenderer _renderer;
		private IPlatform _platform;
		private CommandBuffer _renderCommandBuffer;

		public override void OnReady()
		{
			// Get the Main Camera
			var camService = Engine.Services.Get<CameraService>().Camera;
			_camera = camService;

			// Load Default Resources
			Cursor = UnityEngine.Resources.Load<CursorShapesAsset>( "ImGUI/DefaultCursorShape" );
			Shaders = UnityEngine.Resources.Load<ShaderResourcesAsset>( "ImGUI/DefaultShaders" );
			Style = UnityEngine.Resources.Load<StyleAsset>( "ImGUI/DefaultStyle" );

			// Create the Context
			_context = UImGuiUtility.CreateContext();

			// OnEnable
			void Fail( string reason )
			{
				throw new( $"Failed to start: {reason}." );
			}

			_renderCommandBuffer = RenderUtility.GetCommandBuffer( Constants.UImGuiCommandBuffer );
			_camera.AddCommandBuffer( CameraEvent.AfterEverything, _renderCommandBuffer );

			UImGuiUtility.SetCurrentContext( _context );

			var io = ImGui.GetIO();

			_initialConfiguration.ApplyTo( io );
			Style.ApplyTo( ImGui.GetStyle() );

			_context.TextureManager.BuildFontAtlas( io, _fontAtlasConfiguration, _fontCustomInitializer );
			_context.TextureManager.Initialize( io );

			var platform = PlatformUtility.Create( _platformType, Cursor, _iniSettings );
			SetPlatform( platform, io );
			if ( _platform == null )
			{
				Fail( nameof( _platform ) );
			}

			SetRenderer( RenderUtility.Create( _rendererType, Shaders, _context.TextureManager ), io );
			if ( _renderer == null )
			{
				Fail( nameof( _renderer ) );
			}

			UImGuiUtility.DoOnInitialize( this );
		}

		public override void OnUpdate()
		{
			UImGuiUtility.SetCurrentContext( _context );
			var io = ImGui.GetIO();

			Constants.PrepareFrameMarker.Begin();
			_context.TextureManager.PrepareFrame( io );
			_platform.PrepareFrame( io, _camera.pixelRect );
			ImGui.NewFrame();
#if !UIMGUI_REMOVE_IMGUIZMO
			ImGuizmoNET.ImGuizmo.BeginFrame();
#endif
			Constants.PrepareFrameMarker.End();
			Constants.LayoutMarker.Begin();

			try
			{
				UImGuiUtility.DoLayout( this );
				Callback.Run( "imgui.layout" );
			}
			catch ( Exception e )
			{
				Dev.Log.Exception( e );
			}
			finally
			{
				ImGui.Render();
				Constants.LayoutMarker.End();
			}

			Constants.DrawListMarker.Begin();
			_renderCommandBuffer.Clear();
			_renderer.RenderDrawLists( _renderCommandBuffer, ImGui.GetDrawData() );
			Constants.DrawListMarker.End();
		}

		public override void OnShutdown()
		{
			// OnDisable
			UImGuiUtility.SetCurrentContext( _context );
			var io = ImGui.GetIO();

			SetRenderer( null, io );
			SetPlatform( null, io );

			UImGuiUtility.SetCurrentContext( null );

			_context.TextureManager.Shutdown();
			_context.TextureManager.DestroyFontAtlas( io );

			if ( _camera != null )
			{
				_camera.RemoveCommandBuffer( CameraEvent.AfterEverything, _renderCommandBuffer );
			}

			if ( _renderCommandBuffer != null )
			{
				RenderUtility.ReleaseCommandBuffer( _renderCommandBuffer );
			}

			_renderCommandBuffer = null;

			UImGuiUtility.DoOnDeinitialize( this );

			// Finish
			UImGuiUtility.DestroyContext( _context );
		}

		public void SetUserData( IntPtr userDataPtr )
		{
			_initialConfiguration.UserData = userDataPtr;
			var io = ImGui.GetIO();
			_initialConfiguration.ApplyTo( io );
		}

		private void SetRenderer( IRenderer renderer, ImGuiIOPtr io )
		{
			_renderer?.Shutdown( io );
			_renderer = renderer;
			_renderer?.Initialize( io );
		}

		private void SetPlatform( IPlatform platform, ImGuiIOPtr io )
		{
			_platform?.Shutdown( io );
			_platform = platform;
			_platform?.Initialize( io, _initialConfiguration, "Unity " + _platformType.ToString() );
		}

		// Fields

		[SerializeField]
		private Camera _camera = null;

		[SerializeField]
		private RenderType _rendererType = RenderType.Mesh;

		[SerializeField]
		private InputType _platformType = InputType.InputManager;

		[Tooltip( "Null value uses default imgui.ini file." ), SerializeField]
		private IniSettingsAsset _iniSettings = null;

		[Header( "Configuration" ), SerializeField]
		private UIOConfig _initialConfiguration = new()
		{
			ImGuiConfig = ImGuiConfigFlags.NavEnableKeyboard | ImGuiConfigFlags.DockingEnable,
			DoubleClickTime = 0.30f,
			DoubleClickMaxDist = 6.0f,
			DragThreshold = 6.0f,
			KeyRepeatDelay = 0.250f,
			KeyRepeatRate = 0.050f,
			FontGlobalScale = 1.0f,
			FontAllowUserScaling = false,
			DisplayFramebufferScale = Vector2.one,
			MouseDrawCursor = false,
			TextCursorBlink = false,
			ResizeFromEdges = true,
			MoveFromTitleOnly = true,
			ConfigMemoryCompactTimer = 1f
		};

		[SerializeField]
		private FontInitializerEvent _fontCustomInitializer = new();

		[SerializeField]
		private FontAtlasConfigAsset _fontAtlasConfiguration = null;
	}

}
