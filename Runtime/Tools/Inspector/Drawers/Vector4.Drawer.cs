using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( Vector4 ) )]
	internal class Vector4Drawer : Inspector.Drawer
	{
		public override void OnLayout( Property item, object instance )
		{
			var currentValue = item[instance];

			if ( currentValue == null )
			{
				ImGui.Text( "Null" );
				return;
			}

			var lastValue = (Vector4)currentValue;
			var value = lastValue;

			ImGui.InputFloat4( string.Empty, ref value );

			if ( value != lastValue )
			{
				item[instance] = value;
			}
		}
	}
}
