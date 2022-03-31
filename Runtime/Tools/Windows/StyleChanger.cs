using ImGuiNET;

namespace Espionage.Engine.Tools
{
	public class StyleChanger : Window
	{
		public override void OnLayout()
		{
			ImGui.ShowStyleSelector( "Current" );
			ImGui.Separator();
			ImGui.ShowStyleEditor();
		}
	}
}
