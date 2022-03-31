using Espionage.Engine.Services;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	[Target( typeof( object ) )]
	internal class ObjectDrawer : Inspector.Drawer
	{
		public override bool OnLayout( object instance, in object value, out object change )
		{
			if ( value == null )
			{
				ImGui.Text( "Null" );

				change = null;
				return false;
			}

			if ( ImGui.Selectable( value.ToString() ) )
			{
				Engine.Services.Get<DiagnosticsService>().Selection = value;
			}

			change = null;
			return false;
		}
	}
}
