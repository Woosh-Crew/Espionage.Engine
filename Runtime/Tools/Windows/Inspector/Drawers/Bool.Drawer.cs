using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( bool ) )]
	internal class BoolDrawer : Inspector.Drawer<bool>
	{
		protected override bool OnLayout( object instance, in bool value, out bool change )
		{
			var newValue = value;

			ImGui.Checkbox( string.Empty, ref newValue );
			ImGui.SameLine();
			ImGui.TextColored( Color.gray, value ? "Enabled" : "Disabled" );

			if ( value != newValue )
			{
				change = newValue;
				return true;
			}

			change = default;
			return false;
		}
	}
}
