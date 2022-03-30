using System;
using ImGuiNET;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Espionage.Engine.Tools
{
	public sealed class LightExplorer : Window
	{
		private Light[] Lights { get; set; }

		private void Refresh()
		{
			Lights = Object.FindObjectsOfType<Light>();
		}

		public override void OnLayout()
		{
			if ( Lights == null )
			{
				Refresh();
			}

			// Functions
			if ( ImGui.BeginTable( "Lights", 5, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.PreciseWidths ) )
			{
				ImGui.TableSetupColumn( "Enabled", ImGuiTableColumnFlags.WidthFixed, 48 );
				ImGui.TableSetupColumn( "Light" );
				ImGui.TableSetupColumn( "Type" );
				ImGui.TableSetupColumn( "Color" );
				ImGui.TableSetupColumn( "Intensity" );

				ImGui.TableHeadersRow();

				for ( var index = 0; index < Lights.Length; index++ )
				{
					ImGui.PushID( index );

					var light = Lights[index];
					LightGUI( light, index );

					ImGui.PopID();
				}
			}

			ImGui.EndTable();
		}

		private void LightGUI( Light light, int id )
		{
			// Enabled
			ImGui.TableNextColumn();
			{
				ImGui.SetNextItemWidth( ImGui.GetColumnWidth() );

				var value = light.enabled;
				ImGui.PushID( id * 2 );
				ImGui.Checkbox( string.Empty, ref value );
				ImGui.PopID();
				
				if ( value != light.enabled )
				{
					light.enabled = value;
				}
			}

			// Item Name
			ImGui.TableNextColumn();
			{
				ImGui.SetNextItemWidth( ImGui.GetColumnWidth() );
				if ( ImGui.Selectable( light.name ) )
				{
					Service.Selection = light;
				}
			}

			// Light Type
			ImGui.TableNextColumn();
			{
				ImGui.SetNextItemWidth( ImGui.GetColumnWidth() );
				ImGui.BeginGroup();
				{
					if ( ImGui.Selectable( light.type.ToString() ) ) { }

					// This is stupid.. Who Cares!
					if ( ImGui.BeginPopupContextItem( $"enum_choice_{light.name}", ImGuiPopupFlags.MouseButtonLeft ) )
					{
						ImGui.Text( nameof( LightType ) );
						ImGui.Separator();

						foreach ( var name in Enum.GetNames( typeof( LightType ) ) )
						{
							if ( ImGui.Selectable( name ) )
							{
								light.type = (LightType)Enum.Parse( typeof( LightType ), name );
							}
						}

						ImGui.EndPopup();
					}
				}
				ImGui.EndGroup();
			}

			// Color
			ImGui.TableNextColumn();
			{
				ImGui.SetNextItemWidth( ImGui.GetColumnWidth() );

				// This makes no sense. ColorEdit 4 expects a 0 - 255 value,
				// Where as Color 3 expects a 0 - 1 ? 
				ImGui.BeginGroup();
				{
					var current = light.color;
					var lastValue = new Vector3( current.r, current.g, current.b );
					var value = lastValue;

					ImGui.ColorEdit3( string.Empty, ref value );

					if ( value != lastValue )
					{
						light.color = new( value.x, value.y, value.z );
					}
				}
				ImGui.EndGroup();
			}

			// Intensity
			ImGui.TableNextColumn();
			{
				ImGui.SetNextItemWidth( ImGui.GetColumnWidth() );
				ImGui.BeginGroup();
				{
					var current = light.intensity;
					var value = current;

					ImGui.SliderFloat( string.Empty, ref value, 0, 5 );

					if ( current != value )
					{
						light.intensity = value;
					}
				}
				ImGui.EndGroup();
			}
		}
	}
}
