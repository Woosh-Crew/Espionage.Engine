using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	public class ServicesLookup : Window
	{
		public override void OnLayout()
		{
			ImGui.BeginChild( "Output", new( 0, 0 ), true, ImGuiWindowFlags.ChildWindow );
			{
				foreach ( var service in Engine.Services )
				{
					if ( ImGui.Selectable( $"{service.ClassInfo.Title}" ) )
					{
						Service.Selection = service;
					}

					ImGui.TextColored( Color.gray, $"ClassInfo: [{service.ClassInfo.Name}] - [{service.ClassInfo.Group}]" );
					ImGui.TextColored( Color.gray, $"Stopwatch: [Ready = {service.Time}ms]" );
					ImGui.Separator();
				}
			}
			ImGui.EndChild();
		}
	}
}
