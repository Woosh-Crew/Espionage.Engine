using System;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( Array ) )]
	public class ArrayDrawer : Inspector.Drawer<Array>
	{
		protected override bool OnLayout( object instance, in Array value, out Array change )
		{
			var tree = ImGui.TreeNode( Property.Name );
			ImGui.SameLine();
			ImGui.TextColored( Color.gray, $"[Length: {value.Length}]" );

			if ( tree )
			{
				var underlyingType = Type.GetElementType();

				try
				{
					for ( var i = 0; i < value.Length; i++ )
					{
						var item = value.GetValue( i );

						// Normal Drawer
						ImGui.BeginGroup();
						{
							ImGui.PushID( Property.Name + i );

							if ( Inspector.PropertyGUI( underlyingType, Property, instance, item, out var changed ) )
							{
								value.SetValue( changed, i );
							}

							ImGui.PopID();
						}
						ImGui.EndGroup();

						if ( ImGui.IsItemHovered() && !string.IsNullOrWhiteSpace( Property.Help ) )
						{
							ImGui.SetTooltip( Property.Help );
						}
					}
				}
				finally
				{
					if ( value.Length == 0 )
					{
						ImGui.Text( "Empty Array" );
					}
				}

				ImGui.TreePop();
			}

			change = null;
			return false;
		}
	}
}
