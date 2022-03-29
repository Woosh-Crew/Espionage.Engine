using System;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( Color ) )]
	internal class ColorDrawer : Inspector.Drawer
	{
		public override void OnLayout( Property item, object instance )
		{
			var currentValue = item[instance];

			if ( currentValue == null )
			{
				ImGui.Text( "Null" );
				return;
			}
			
			var lastValue = (Color)currentValue;

			var value = new Vector4( lastValue.r * 255, lastValue.b * 255, lastValue.g * 255, lastValue.a );
			ImGui.ColorEdit4( string.Empty, ref value );
			
			if ( value != (Vector4)lastValue )
			{
				item[instance] = new Color(value.x /255, value.y /255, value.z /255, value.w);
			}
		}
	}
}
