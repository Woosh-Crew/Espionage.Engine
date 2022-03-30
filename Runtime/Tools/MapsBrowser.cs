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
				foreach ( var entry in Map.Database )
				{
					if ( ImGui.Selectable( entry.Components.TryGet<Meta>( out var meta ) ? meta.Title : entry.Identifier ) )
					{
						Service.Selection = entry;
					}

					if ( ImGui.BeginPopupContextItem() )
					{
						if ( ImGui.Button( "Load" ) )
						{
							Engine.Game.Loader.Start( entry );
						}

						if ( ImGui.Button( "Close" ) )
						{
							ImGui.CloseCurrentPopup();
						}

						ImGui.EndPopup();
					}

					if ( ImGui.IsItemHovered() && meta != null )
					{
						ImGui.BeginTooltip();
						ImGui.Text( entry.Identifier );

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
