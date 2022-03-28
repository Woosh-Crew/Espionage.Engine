using Espionage.Engine.Services;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( Library ) )]
	internal class ClassInfoDrawer : Inspector.Drawer
	{
		public override void OnLayout( Property item, ILibrary instance )
		{
			var lib = item[instance] as Library;
			
			if ( ImGui.Selectable( "Library" ) )
			{
				Engine.Services.Get<DiagnosticsService>().Selection = lib;
			}
			
			ImGui.SameLine();
			ImGui.TextColored(Color.gray, $" [{lib.Name}, {lib.Group}]");
		}
	}
}
