using System;
using System.Linq;
using System.Reflection;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools.Editors
{
	[Target( typeof( object ) )]
	internal class ObjectEditor : Inspector.Editor
	{
		public override void OnActive( object item )
		{
			_fields = item.GetType().GetFields( BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic )
				.Where( e => !e.IsDefined( typeof( ObsoleteAttribute ) ) )
				.ToArray();

			_properties = item.GetType().GetProperties( BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic )
				.Where( e => !e.IsDefined( typeof( ObsoleteAttribute ) ) )
				.ToArray();
		}

		private FieldInfo[] _fields;
		private PropertyInfo[] _properties;

		//
		// User Interface
		//

		public override void OnHeader( object item )
		{
			ImGui.Text( item.GetType().Name );
			ImGui.SameLine();
			ImGui.TextColored( Color.gray, $"[{item.GetType().Name} / {item.GetType().Namespace}]" );

			ImGui.Text( item.ToString() );
		}

		public override void OnLayout( object item )
		{
			if ( ImGui.BeginChild( "out", new( 0, ImGui.GetWindowHeight() - 96 ), false ) )
			{
				if ( ImGui.TreeNodeEx( "Properties", ImGuiTreeNodeFlags.DefaultOpen ) )
				{
					ImGui.Unindent();

					// Properties
					if ( ImGui.BeginTable( "table_fields", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable ) )
					{
						ImGui.TableSetupColumn( "Name", ImGuiTableColumnFlags.WidthFixed, 96 );
						ImGui.TableSetupColumn( "Value" );

						ImGui.TableHeadersRow();

						foreach ( var property in _properties )
						{
							ImGui.TableNextColumn();
							ImGui.Text( property.Name );

							ImGui.TableNextColumn();
							ImGui.SetNextItemWidth( ImGui.GetColumnWidth( 1 ) );

							ImGui.TextDisabled( property.GetValue( item )?.ToString() ?? "Null" );
							if ( ImGui.IsItemHovered() )
							{
								ImGui.SetTooltip( "Item is Read Only, since this object is just a straight object" );
							}
						}

						ImGui.EndTable();
					}

					ImGui.Indent();
					ImGui.TreePop();
				}

				if ( ImGui.TreeNodeEx( "Fields", ImGuiTreeNodeFlags.DefaultOpen ) )
				{
					ImGui.Unindent();

					// Properties
					if ( ImGui.BeginTable( "table_fields", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable ) )
					{
						ImGui.TableSetupColumn( "Name", ImGuiTableColumnFlags.WidthFixed, 96 );
						ImGui.TableSetupColumn( "Value" );

						ImGui.TableHeadersRow();

						foreach ( var field in _fields )
						{
							ImGui.TableNextColumn();
							ImGui.Text( field.Name );

							ImGui.TableNextColumn();
							ImGui.SetNextItemWidth( ImGui.GetColumnWidth( 1 ) );

							ImGui.TextDisabled( field.GetValue( item )?.ToString() ?? "Null" );
							if ( ImGui.IsItemHovered() )
							{
								ImGui.SetTooltip( "Item is Read Only, since this object is just a straight object" );
							}
						}

						ImGui.EndTable();
					}

					ImGui.Indent();
					ImGui.TreePop();
				}
			}
		}
	}
}
