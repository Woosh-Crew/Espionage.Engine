using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Group( "Overlays" )]
	public abstract class Overlay : Window
	{
		internal static float offset;
		internal static int index;

		internal override bool Layout()
		{
			index++;

			var delete = true;

			const ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoNav;

			const float padding = 16;
			var viewport = ImGui.GetMainViewport();
			var workPos = viewport.WorkPos;
			var windowPos = new Vector2 { x = workPos.x + viewport.Size.x - padding, y = workPos.y + padding + offset + 4 * index };

			ImGui.SetNextWindowPos( windowPos, ImGuiCond.Always, new( 1f, 0f ) );

			ImGui.SetNextWindowBgAlpha( 0.35f );
			if ( ImGui.Begin( ClassInfo.Title, ref delete, windowFlags ) )
			{
				OnLayout();
			}

			offset += ImGui.GetWindowSize().y;

			ImGui.End();

			return !delete;
		}
	}
}
