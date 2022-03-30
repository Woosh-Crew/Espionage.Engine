using System.Collections;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( IEnumerable ) )]
	public class IEnumerableDrawer : Inspector.Drawer<IEnumerable>
	{
		protected override bool OnLayout( object instance, in IEnumerable value, out IEnumerable change )
		{
			if ( Property.IsStatic )
			{
				ImGui.Text( "Static Property. Not Enumerating" );

				change = null;
				return false;
			}
			
			var tree = ImGui.TreeNode( Property.Name ) ;
			ImGui.SameLine();
			ImGui.TextColored(Color.gray, "[Readonly]");

			if ( tree )
			{
				var index = 0;

				try
				{
					foreach ( var item in value )
					{
						index++;

						if ( item == null )
						{
							ImGui.Text( "Null" );
							continue;
						}

						// Normal Drawer
						ImGui.BeginGroup();
						{
							ImGui.PushID( Property.Name + index );

							Inspector.PropertyGUI( value.GetType().GetGenericArguments()[0], Property, instance, item, out var hasChanged, out var changed );

							if ( hasChanged )
							{
								Dev.Log.Info( "Value Changed" );
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
					if ( index == 0 )
					{
						ImGui.Text( "Empty Collection" );
					}
				}

				ImGui.TreePop();
			}

			change = null;
			return false;
		}
	}
}
