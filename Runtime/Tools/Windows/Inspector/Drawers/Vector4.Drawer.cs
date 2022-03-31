using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( Vector4 ) )]
	internal class Vector4Drawer : Inspector.Drawer<Vector4>
	{
		protected override bool OnLayout( object instance, in Vector4 value, out Vector4 change )
		{
			var newValue = value;

			ImGui.InputFloat4( string.Empty, ref newValue );

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
