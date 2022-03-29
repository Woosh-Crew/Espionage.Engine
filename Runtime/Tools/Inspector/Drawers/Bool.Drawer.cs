using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( bool ) )]
	internal class BoolDrawer : Inspector.Drawer
	{
		public override void OnLayout( Property item, object instance )
		{
			var currentValue = item[instance];

			if ( currentValue == null )
			{
				ImGui.Text( "Null" );
				return;
			}

			var lastValue = (bool)currentValue;
			var value = lastValue;

			ImGui.Checkbox( string.Empty, ref value );

			ImGui.SameLine();
			ImGui.TextColored( Color.gray, value ? "Enabled" : "Disabled" );

			if ( value != lastValue )
			{
				item[instance] = value;
			}
		}
	}
}
