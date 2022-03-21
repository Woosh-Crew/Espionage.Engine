using System;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	public class Inspector : Tool
	{
		public override void OnLayout()
		{
			// Don't render if there is nothing there.
			if ( Service.Selection == null )
			{
				ImGui.Text( "Nothing Selected!" );
				return;
			}

			var ent = Service.Selection;

			// Render Class Info
			ImGui.Text( $"{ent.ClassInfo.Title}" );
			ImGui.Text( $"{ent.ClassInfo.Name} - [{ent.ClassInfo.Group}]" );
			ImGui.Separator();
			ImGui.TextColored( Color.green, "Entity" );
			ImGui.BeginChild( "Entity Values", new( 0, 128 ), true, ImGuiWindowFlags.ChildWindow );
			{
				ImGui.Text( "this would render properties" );
			}
			ImGui.EndChild();

			ImGui.TextColored( Color.green, "Components" );

			ImGui.BeginChild( "Components", new( 0, 0 ), true, ImGuiWindowFlags.ChildWindow );
			{
				for ( var i = 0; i < ent.Components.Count; i++ )
				{
					ComponentUI( ent, ent.Components[i] as Component, i );

				}
			}
			ImGui.EndChild();


		}

		private void ComponentUI( Entity selction, Component component, int index )
		{
			if ( ImGui.TreeNode( new IntPtr( index ), $"{component.ClassInfo.Name} - [{component.ClassInfo.Group}]" ) )
			{

				ImGui.TreePop();
			}
		}
	}
}
