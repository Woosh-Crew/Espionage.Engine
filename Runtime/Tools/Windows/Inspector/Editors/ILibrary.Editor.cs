using System;
using System.Linq;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools.Editors
{
	[Target( typeof( ILibrary ) )]
	public class ILibraryEditor : Inspector.Editor<ILibrary>
	{
		protected override void OnActive( ILibrary item )
		{
			_search = string.Empty;
		}

		//
		// User Interface
		//

		public override void OnHeader( ILibrary item )
		{
			ImGui.Text( item.ClassInfo.Title );
			ImGui.SameLine();
			ImGui.TextColored( Color.gray, $"[{item.ClassInfo.Name} / {item.ClassInfo.Group}]" );

			ImGui.Text( item.ToString() );
		}

		private string _search = string.Empty;

		protected override void OnLayout( ILibrary item )
		{
			if ( item.ClassInfo == null )
			{
				ImGui.TextColored( Color.red, "NULL ClassInfo" );
				return;
			}

			ImGui.SetNextItemWidth( ImGui.GetWindowWidth() - 16 );
			ImGui.InputTextWithHint( "Search", "Member Search...", ref _search, 160 );

			// Us doing this removes the title.. but we gotta or else the scrolling just doesnt work
			if ( ImGui.BeginChild( "out", new( 0, ImGui.GetWindowHeight() - 96 ), false ) )
			{
				if ( ImGui.TreeNodeEx( "Properties", ImGuiTreeNodeFlags.DefaultOpen ) )
				{
					ImGui.Unindent();

					// Properties
					if ( ImGui.BeginTable( "table_properties", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable ) )
					{
						ImGui.TableSetupColumn( "Name", ImGuiTableColumnFlags.WidthFixed, 96 );
						ImGui.TableSetupColumn( "Value" );

						ImGui.TableHeadersRow();

						foreach ( var property in string.IsNullOrEmpty( _search )
							         ? item.ClassInfo.Properties
							         : item.ClassInfo.Properties.Where( e => e.Name.Contains( _search, StringComparison.CurrentCultureIgnoreCase ) ) )
						{
							ImGui.TableNextColumn();
							ImGui.Text( property.Title );

							ImGui.TableNextColumn();
							ImGui.SetNextItemWidth( ImGui.GetColumnWidth( 1 ) );
							Inspector.PropertyGUI( property, item );
						}

						ImGui.EndTable();
					}

					ImGui.Indent();
					ImGui.TreePop();
				}


				ImGui.Separator();

				if ( ImGui.TreeNodeEx( "Functions", ImGuiTreeNodeFlags.DefaultOpen ) )
				{
					ImGui.Unindent();

					if ( ImGui.BeginTable( "table_functions", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable ) )
					{
						ImGui.TableSetupColumn( "Name", ImGuiTableColumnFlags.WidthFixed, 96 );
						ImGui.TableSetupColumn( "Function" );

						ImGui.TableHeadersRow();

						foreach ( var function in string.IsNullOrEmpty( _search )
							         ? item.ClassInfo.Functions
							         : item.ClassInfo.Functions.Where( e => e.Name.Contains( _search, StringComparison.CurrentCultureIgnoreCase ) ) )
						{
							ImGui.TableNextColumn();
							ImGui.Text( function.Title );

							ImGui.TableNextColumn();
							ImGui.SetNextItemWidth( ImGui.GetColumnWidth( 1 ) );

							ImGui.PushID( function.Title );
							if ( ImGui.Button( "Invoke" ) )
							{
								Debugging.Log.Info( $"Invoking {function.Title}" );
								function.Invoke( item );
							}

							ImGui.PopID();
						}

						ImGui.EndTable();
					}

					ImGui.Indent();
					ImGui.TreePop();
				}
			}

			ImGui.EndChild();
		}
	}
}
