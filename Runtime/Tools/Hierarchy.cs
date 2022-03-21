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
					if ( ImGui.Selectable( $"{entity.name} [{entity.ClassInfo.Title}]" ) )
					{
						Service.Selection = entity;
					}
				}
			}
			ImGui.EndChild();
		}
	}
}
