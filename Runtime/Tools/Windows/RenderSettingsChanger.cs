using System.IO;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	public class RenderSettingsChanger : Window
	{
		public override void OnLayout()
		{
			// Fog Enabled
			{
				var value = RenderSettings.fog;
				ImGui.Checkbox( "Enable Fog", ref value );

				if ( value != RenderSettings.fog )
				{
					RenderSettings.fog = value;
				}
			}
			
			// Fog Density
			{
				var value = RenderSettings.fogDensity;
				ImGui.SliderFloat( "Fog Density", ref value, 0, 1 );

				if ( value != RenderSettings.fogDensity )
				{
					RenderSettings.fogDensity = value;
				}
			}
			
			// Fog Color
			{
				var value = RenderSettings.fogColor;
				var newValue = new Vector4( value.r, value.g, value.b, value.a );
				ImGui.ColorEdit4( "Fog Color", ref newValue );

				if ( (Vector4)value != newValue )
				{
					RenderSettings.fogColor = new( newValue.x, newValue.y , newValue.z , newValue.w );
				}
			}
		}
	}
}
