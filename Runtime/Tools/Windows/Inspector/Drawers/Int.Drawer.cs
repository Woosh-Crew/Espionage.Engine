using System;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	[Target( typeof( int ) )]
	internal class IntDrawer : Inspector.Drawer<int>
	{
		protected override bool OnLayout( object instance, in int value, out int change )
		{
			var newValue = value;

			// GUI
			if ( Property != null && Property.Components.TryGet<SliderAttribute>( out var attribute ) )
			{
				ImGui.SliderInt( string.Empty, ref newValue, (int)attribute.Min, (int)attribute.Max );
			}
			else
			{
				ImGui.InputInt( string.Empty, ref newValue );
			}

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
