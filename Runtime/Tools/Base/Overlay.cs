using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Group( "Overlays" )]
	public abstract class Overlay : Window
	{
		public static float Offset;
		public static int Index;

		internal override bool Layout()
		{
			Index++;

			var delete = true;

			const ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoNav;

			const float padding = 16;
			var viewport = ImGui.GetMainViewport();
			var workPos = viewport.WorkPos;
			var windowPos = new Vector2 { x = workPos.x + padding, y = viewport.Size.y - padding - (padding - Offset) - 8 * Index };

			ImGui.SetNextWindowPos( windowPos, ImGuiCond.Always );

			ImGui.SetNextWindowBgAlpha( 0.35f );
			if ( ImGui.Begin( ClassInfo.Title, ref delete, windowFlags ) )
			{
				OnLayout();
			}

			Offset += ImGui.GetWindowSize().y;

			ImGui.End();

			return !delete;
		}
	}
}
