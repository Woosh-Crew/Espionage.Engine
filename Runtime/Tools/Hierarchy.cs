using ImGuiNET;

namespace Espionage.Engine.Tools
{
	public class Hierarchy : Window
	{
		public override void OnLayout()
		{
			ImGui.BeginChild( "Output", new( 0, 0 ), true, ImGuiWindowFlags.ChildWindow );
			{
				foreach ( var entity in Entity.All )
				{
					var opened = ImGui.TreeNodeEx( entity.name, ImGuiTreeNodeFlags.OpenOnArrow );

					if ( ImGui.IsItemClicked() )
					{
						Service.Selection = entity;
					}

					if ( opened )
					{
						ImGui.TreePop();
					}
				}
			}
			ImGui.EndChild();
		}
	}
}
