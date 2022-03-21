﻿using System;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	public class Inspector : Window
	{
		public override void OnLayout()
		{
			// Don't render if there is nothing there.
			if ( Service.Selection == null )
			{
				ImGui.Text( "Nothing Selected!" );
				return;
			}

			var lib = Service.Selection;

			// Entity Class Info
			ImGui.Text( $"{lib.ClassInfo.Title}" );
			ImGui.Text( $"{lib.ClassInfo.Name} - [{lib.ClassInfo.Group}]" );

			if ( Service.Selection is Pawn pawn && ImGui.Button( "Possess Pawn" ) )
			{
				Local.Client.Pawn = pawn;
				Dev.Terminal.Invoke( "dev.tripod" );
				Controls.Cursor.Locked = false;
			}

			ImGui.Separator();

			// Entity Property Inspector
			ImGui.TextColored( Color.green, "Properties" );
			ImGui.BeginChild( $"{lib.ClassInfo.Title} Values", new( 0, lib.ClassInfo.Properties.Count * (ImGui.GetFontSize() + 16) + 2 ), true, ImGuiWindowFlags.ChildWindow );
			{
				foreach ( var property in lib.ClassInfo.Properties.All )
				{
					PropertyUI( property, lib );
				}
			}
			ImGui.EndChild();

			ImGui.TextColored( Color.green, "Functions" );
			// Entity Functions
			ImGui.BeginChild( $"{lib.ClassInfo.Title} Functions", new( 0, lib.ClassInfo.Functions.Count * (ImGui.GetFontSize() + 16) + 2 ), true, ImGuiWindowFlags.ChildWindow );
			{
				foreach ( var function in lib.ClassInfo.Functions.All )
				{
					FunctionUI( function, lib );
				}
			}
			ImGui.EndChild();

			ImGui.Separator();

			if ( lib is Entity entity )
			{
				// Component Property Inspector
				ImGui.TextColored( Color.green, "Components" );
				ImGui.BeginChild( "Entity Components", new( 0, entity.Components.Count * (ImGui.GetFontSize() + 16) + 2 ), true, ImGuiWindowFlags.ChildWindow );
				{
					for ( var i = 0; i < entity.Components.Count; i++ )
					{
						ComponentUI( entity.Components[i] as Component, i );
					}
				}
				ImGui.EndChild();
			}
		}

		private void ComponentUI( Component component, int index )
		{
			if ( component == null )
			{
				return;
			}

			if ( ImGui.TreeNode( new IntPtr( index ), $"{component.ClassInfo.Name} - [{component.ClassInfo.Group}]" ) )
			{
				foreach ( var property in component.ClassInfo.Properties.All )
				{
					PropertyUI( property, component );
				}

				ImGui.TreePop();
			}

			if ( ImGui.IsItemHovered() && !string.IsNullOrEmpty( component.ClassInfo.Help ) )
			{
				ImGui.SetTooltip( component.ClassInfo.Help );
			}
		}

		private void PropertyUI( Property property, ILibrary scope )
		{
			ImGui.Text( $"{property.Name} = [{property[scope]}]" );

			if ( ImGui.IsItemHovered() && !string.IsNullOrEmpty( property.Help ) )
			{
				ImGui.SetTooltip( property.Help );
			}
		}

		private void FunctionUI( Function function, ILibrary scope )
		{
			ImGui.Text( $"{function.Name}" );

			if ( ImGui.IsItemHovered() && !string.IsNullOrEmpty( function.Help ) )
			{
				ImGui.SetTooltip( function.Help );
			}
		}
	}
}
