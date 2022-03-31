using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ImGuiNET;
using Object = UnityEngine.Object;

namespace Espionage.Engine.Tools.Editors
{
	[Target( typeof( Object ) )]
	internal class ObjectEditor : Inspector.Editor<Object>
	{
		protected override void OnActive( Object item )
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

		public override void OnHeader( Object item )
		{
			ImGui.TextWrapped( "You shouldn't be editing UnityEngine.Object, its here anyway so use at your own risk." );
		}

		protected override void OnLayout( Object item )
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

							if ( Inspector.PropertyGUI( property.PropertyType, null, item, property.GetValue( item ), out var changed ) )
							{
								property.SetValue( item, changed );
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

							if ( Inspector.PropertyGUI( field.FieldType, null, item, field.GetValue( item ), out var changed ) )
							{
								field.SetValue( item, changed );
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
