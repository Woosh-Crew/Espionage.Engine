using Espionage.Engine.Services;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	public class ILibraryDrawer : Inspector.Drawer
	{
		public override void OnLayout( Property property, ILibrary instance )
		{
			var currentValue = property[instance];

			if ( currentValue == null )
			{
				ImGui.Text( "Null" );
				return;
			}

			var castedValue = (ILibrary)currentValue;
			
			if ( ImGui.Selectable( castedValue.ClassInfo.Title ) )
			{
				Engine.Services.Get<DiagnosticsService>().Selection = castedValue;
			}
		}
	}
}
