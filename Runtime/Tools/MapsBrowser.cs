using Espionage.Engine.Resources;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	public class MapsBrowser : Window
	{
		public override void OnLayout()
		{
			// Maps!
			ImGui.BeginChild( "Output", new( 0, 0 ), true, ImGuiWindowFlags.ChildWindow );
			{
				foreach ( var entry in Map.Database.All )
				{
					if ( ImGui.Selectable(entry.Identifier) )
					{
						Service.Selection = entry;
					}
					
					if ( ImGui.IsItemHovered() && entry.Components.TryGet<Meta>( out var meta ) )
					{
						ImGui.BeginTooltip();
						if ( !string.IsNullOrEmpty( meta.Title ) )
						{
							ImGui.Text( meta.Title );
						}

						if ( !string.IsNullOrEmpty( meta.Description ) )
						{
							ImGui.Text( meta.Description );
						}

						if ( !string.IsNullOrEmpty( meta.Author ) )
						{
							ImGui.Text( meta.Author );
						}

						ImGui.EndTooltip();
					}
				}
			}
			ImGui.EndChild();

		}
	}
}
