using System;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools.Editors
{
	[Target( typeof( Light ) )]
	internal class LightEditor : Inspector.Editor<Light>
	{
		protected override void OnLayout( Light item )
		{
			// Transform

			// Light

			// Type
			ImGui.PushID( "light.type" );
			{
				if ( Inspector.PropertyGUI( typeof( LightType ), null, item, item.type, out var newValue ) )
				{
					item.type = (LightType)newValue;
				}
			}
			ImGui.PopID();

			if ( item.type is LightType.Point or LightType.Spot )
			{
				// Light Range
				ImGui.PushID( "light.range" );
				{
					if ( Inspector.PropertyGUI( typeof( float ), null, item, item.range, out var newValue ) )
					{
						item.range = (float)newValue;
					}
				}
				ImGui.PopID();


				if ( item.type is LightType.Spot )
				{
					// Spot Angle - Hardcoded for Slider
					ImGui.PushID( "light.spot_angle" );
					{
						var newValue = item.spotAngle;

						// GUI
						ImGui.SliderFloat( string.Empty, ref newValue, 0, 180 );

						if ( Math.Abs( newValue - item.spotAngle ) > 0.0001f )
						{
							item.spotAngle = newValue;
						}
					}
					ImGui.PopID();
				}
			}

			// Color
			ImGui.PushID( "light.color" );
			{
				if ( Inspector.PropertyGUI( typeof( Color ), null, item, item.color, out var newValue ) )
				{
					item.color = (Color)newValue;
				}
			}
			ImGui.PopID();

			// Bake Mode
			ImGui.PushID( "light.mode" );
			{
				if ( Inspector.PropertyGUI( typeof( LightmapBakeType ), null, item, item.lightmapBakeType, out var newValue ) )
				{
					item.lightmapBakeType = (LightmapBakeType)newValue;
				}
			}
			ImGui.PopID();

			// Intensity
			ImGui.PushID( "light.intensity" );
			{
				if ( Inspector.PropertyGUI( typeof( float ), null, item, item.intensity, out var newValue ) )
				{
					item.intensity = (float)newValue;
				}
			}
			ImGui.PopID();

			// Intensity
			ImGui.PushID( "light.indirect" );
			{
				if ( Inspector.PropertyGUI( typeof( float ), null, item, item.bounceIntensity, out var newValue ) )
				{
					item.bounceIntensity = (float)newValue;
				}
			}
			ImGui.PopID();

			// Intensity
			ImGui.PushID( "light.shadows" );
			{
				if ( Inspector.PropertyGUI( typeof( LightShadows ), null, item, item.shadows, out var newValue ) )
				{
					item.shadows = (LightShadows)newValue;
				}
			}
			ImGui.PopID();
		}
	}
}
