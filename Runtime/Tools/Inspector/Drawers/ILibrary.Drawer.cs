using Espionage.Engine.Services;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	public class ILibraryDrawer : Inspector.Drawer
	{
		public override void OnLayout( Property property, ILibrary instance )
		{
			var currentValue = property[instance];
			var castedValue = (ILibrary)currentValue;

			if ( currentValue == null )
			{
				ImGui.Text( "Null" );
				return;
			}

			if ( ImGui.Selectable( currentValue.ToString() ) )
			{
				Engine.Services.Get<DiagnosticsService>().Selection = castedValue;
			}

			ImGui.SameLine();
			ImGui.TextColored( Color.gray, $" [{castedValue.ClassInfo.Title}]" );
		}
	}
}
