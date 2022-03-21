using ImGuiNET;
using System;
using Espionage.Engine.ImGUI.Texture;
using UnityEngine;
using UTexture = UnityEngine.Texture;

namespace Espionage.Engine.ImGUI
{
	public static class UImGuiUtility
	{
		public static IntPtr GetTextureId( UTexture texture )
		{
			return Context?.TextureManager.GetTextureId( texture ) ?? IntPtr.Zero;
		}

		internal static SpriteInfo GetSpriteInfo( Sprite sprite )
		{
			return Context?.TextureManager.GetSpriteInfo( sprite ) ?? null;
		}

		internal static Context Context;

		#region Events

		public static event Action<ImGUIService> Layout;
		public static event Action<ImGUIService> OnInitialize;
		public static event Action<ImGUIService> OnDeinitialize;

		internal static void DoLayout( ImGUIService uimgui )
		{
			Layout?.Invoke( uimgui );
		}

		internal static void DoOnInitialize( ImGUIService uimgui )
		{
			OnInitialize?.Invoke( uimgui );
		}

		internal static void DoOnDeinitialize( ImGUIService uimgui )
		{
			OnDeinitialize?.Invoke( uimgui );
		}

		#endregion

		internal static unsafe Context CreateContext()
		{
			return new()
			{
				ImGuiContext = ImGui.CreateContext(),
#if !UIMGUI_REMOVE_IMPLOT
				ImPlotContext = ImPlotNET.ImPlot.CreateContext(),
#endif
#if !UIMGUI_REMOVE_IMNODES
				ImNodesContext = new( imnodesNET.imnodes.CreateContext() ),
#endif
				TextureManager = new()
			};
		}

		internal static void DestroyContext( Context context )
		{
			ImGui.DestroyContext( context.ImGuiContext );

#if !UIMGUI_REMOVE_IMPLOT
			ImPlotNET.ImPlot.DestroyContext( context.ImPlotContext );
#endif
#if !UIMGUI_REMOVE_IMNODES
			imnodesNET.imnodes.DestroyContext( context.ImNodesContext );
#endif
		}

		internal static void SetCurrentContext( Context context )
		{
			Context = context;
			ImGui.SetCurrentContext( context?.ImGuiContext ?? IntPtr.Zero );

#if !UIMGUI_REMOVE_IMPLOT
			ImPlotNET.ImPlot.SetImGuiContext( context?.ImGuiContext ?? IntPtr.Zero );
#endif
#if !UIMGUI_REMOVE_IMGUIZMO
			ImGuizmoNET.ImGuizmo.SetImGuiContext( context?.ImGuiContext ?? IntPtr.Zero );
#endif
#if !UIMGUI_REMOVE_IMNODES
			imnodesNET.imnodes.SetImGuiContext( context?.ImGuiContext ?? IntPtr.Zero );
#endif
		}
	}
}
