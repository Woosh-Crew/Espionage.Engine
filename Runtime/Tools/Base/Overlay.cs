using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	public abstract class Overlay : Window
	{
		internal override bool Layout()
		{
			var delete = true;

			const ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoNav;

			const float padding = 16;
			var viewport = ImGui.GetMainViewport();
			var workPos = viewport.WorkPos;
			var windowPos = new Vector2 { x = (workPos.x + viewport.WorkSize.x - 128) - padding, y = workPos.y + padding };

			ImGui.SetNextWindowPos( windowPos, ImGuiCond.Always );
			ImGui.SetNextWindowSize( new( 128, 0 ) );

			ImGui.SetNextWindowBgAlpha( 0.35f );
			if ( ImGui.Begin( ClassInfo.Title, ref delete, windowFlags ) )
			{
				OnLayout();
			}

			ImGui.End();

			return !delete;
		}
	}
}
