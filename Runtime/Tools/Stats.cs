using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	public class Stats : Overlay
	{
		public override void OnLayout()
		{
			ImGui.Text($"FPS: {(int)(1 / Time.smoothDeltaTime)}");
			ImGui.Text($"Frame: {Time.frameCount}");
			ImGui.Text($"Time: {Time.time}");
			ImGui.Text($"Time Scale: {Time.timeScale}");
		}
	}
}
