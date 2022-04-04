using System.Linq;
using Espionage.Engine.Internal;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	public class CookiesViewer : Window
	{
		public override void OnLayout()
		{
			if ( ImGui.BeginChild( "_options", new( 0, ImGui.GetWindowHeight() - 96 ), false ) )
			{
				if ( ImGui.BeginTable( "Options", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable ) )
				{
					ImGui.TableSetupColumn( "Option", ImGuiTableColumnFlags.WidthFixed, 96 );
					ImGui.TableSetupColumn( "Value" );

					ImGui.TableHeadersRow();

					foreach ( var option in Library.Global.Properties.Where( e => e.Components.Has<CookieAttribute>() ) )
					{
						ImGui.TableNextColumn();
						ImGui.Text( option.Name );

						if ( ImGui.IsItemHovered() )
						{
							ImGui.SetTooltip( option.Title );
						}

						ImGui.TableNextColumn();
						ImGui.SetNextItemWidth( ImGui.GetColumnWidth( 1 ) );
						Inspector.PropertyGUI( option, null );
					}
				}

				ImGui.EndTable();
				ImGui.Separator();
			}

			ImGui.EndChild();

			if ( ImGui.Button( "Save Cookies" ) )
			{
				Cookies.Save();
			}

			ImGui.SameLine();

			if ( ImGui.Button( "Open Location" ) )
			{
				Files.Open( "config://" );
			}
		}
	}
}
