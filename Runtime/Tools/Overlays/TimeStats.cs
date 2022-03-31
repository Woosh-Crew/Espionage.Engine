using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	public class TimeStats : Overlay
	{
		public override void OnLayout()
		{
			ImGui.Text( $"Time: {(int)Time.time}" );
			ImGui.Text( $"Time Scale: {Time.timeScale}" );
		}
	}
}
