using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( Vector3 ) )]
	internal class Vector3Drawer : Inspector.Drawer
	{
		public override void OnLayout( Property item, object instance )
		{
			var currentValue = item[instance];

			if ( currentValue == null )
			{
				ImGui.Text( "Null" );
				return;
			}

			var lastValue = (Vector3)currentValue;
			var value = lastValue;

			ImGui.InputFloat3( string.Empty, ref value );

			if ( value != lastValue )
			{
				item[instance] = value;
			}
		}
	}
}
