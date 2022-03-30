﻿using System;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	[Target( typeof( float ) )]
	internal class FloatDrawer : Inspector.Drawer
	{
		public override void OnLayout( Property item, object instance )
		{
			var currentValue = item[instance];

			if ( currentValue == null )
			{
				ImGui.Text( "Null" );
				return;
			}

			var lastValue = (float)currentValue;
			var value = lastValue;

			// GUI
			if ( item.Components.TryGet<SliderAttribute>( out var attribute ) )
			{
				ImGui.SliderFloat( string.Empty, ref value, attribute.Min, attribute.Max );
			}
			else
			{
				ImGui.InputFloat( string.Empty, ref value );
			}

			if ( Math.Abs( value - lastValue ) > 0.0001f )
			{
				item[instance] = value;
			}
		}
	}
}
