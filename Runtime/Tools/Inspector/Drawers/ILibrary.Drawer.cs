using Espionage.Engine.Services;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( ILibrary ) )]
	internal class ILibraryDrawer : Inspector.Drawer
	{
		public override void OnLayout( Property property, object instance )
		{
			var currentValue = property[instance];

			if ( currentValue is not ILibrary library )
			{
				ImGui.Text( "Error" );
				return;
			}

			if ( ImGui.Selectable( currentValue.ToString() ) )
			{
				Engine.Services.Get<DiagnosticsService>().Selection = currentValue;
			}

			ImGui.SameLine();
			ImGui.TextColored( Color.gray, $" [{library.ClassInfo.Title}]" );
		}
	}
}
