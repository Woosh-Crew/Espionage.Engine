using System;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	[Target( typeof( float ) )]
	internal class FloatDrawer : Inspector.Drawer<float>
	{
		protected override bool OnLayout( object instance, in float value, out float change )
		{
			var newValue = value;

			// GUI
			if ( Property != null && Property.Components.TryGet<SliderAttribute>( out var attribute ) )
			{
				ImGui.SliderFloat( string.Empty, ref newValue, attribute.Min, attribute.Max );
			}
			else
			{
				ImGui.DragFloat( string.Empty, ref newValue, 0.1f );
			}

			if ( Math.Abs( newValue - value ) > 0.0001f )
			{
				change = newValue;
				return true;
			}

			change = default;
			return false;
		}
	}
}
