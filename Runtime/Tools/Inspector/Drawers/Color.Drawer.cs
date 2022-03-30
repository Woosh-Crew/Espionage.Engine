using System;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( Color ) )]
	internal class ColorDrawer : Inspector.Drawer<Color>
	{
		protected override bool OnLayout( object instance, in Color value, out Color change )
		{
			var newValue = new Vector4( value.r * 255, value.g * 255, value.b * 255, value.a );

			ImGui.ColorEdit4( string.Empty, ref newValue );

			if ( (Vector4)value != newValue )
			{
				change = new( newValue.x / 255, newValue.y / 255, newValue.z / 255, newValue.w );
				return true;
			}

			change = default;
			return false;
		}
	}
}
