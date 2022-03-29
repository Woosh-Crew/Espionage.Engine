using System;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	[Target( typeof( int ) )]
	internal class IntDrawer : Inspector.Drawer
	{
		public override void OnLayout( Property item, object instance )
		{
			var currentValue = item[instance];

			if ( currentValue == null )
			{
				ImGui.Text( "Null" );
				return;
			}
			
			var lastValue = (int)currentValue;
			var value = lastValue;
			
			ImGui.InputInt( string.Empty, ref value );

			if ( value != lastValue )
			{
				item[instance] = value;
			}
		}
	}
}
