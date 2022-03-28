using System;
using System.Collections.Generic;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	[Title( "Inspector" )]
	public partial class Inspector : Window
	{
		public override void OnLayout()
		{
			// Don't render if there is nothing there.
			if ( Service.Selection == null )
			{
				ImGui.Text( "Nothing Selected!" );
				return;
			}

			HeaderGUI();
			ImGui.Separator();

			// Draw UI
			DefaultGUI( Service.Selection );
		}

		public void SelectionChanged( ILibrary selection ) { }

		private void HeaderGUI()
		{
			ImGui.Text( $"Viewing {Service.Selection.ClassInfo.Title}" + (Service.Selection is Library ? " (ClassInfo)" : "") );

			if ( ImGui.IsItemHovered() )
			{
				ImGui.SetTooltip( $"Name : {Service.ClassInfo.Name ?? "NULL"}\nTitle : {Service.ClassInfo.Title ?? "NULL"}\nGroup : {Service.ClassInfo.Group ?? "NULL"}\nHelp : {Service.ClassInfo.Help ?? "NULL"}" );
			}

			if ( ImGui.Selectable( "Class Info" ) )
			{
				Service.Selection = Service.Selection.ClassInfo;
			}

		}

		private void DefaultGUI( ILibrary item )
		{
			// Us doing this removes the title.. but we gotta or else the scrolling just doesnt work
			if ( ImGui.BeginChild( "out", new( 0, ImGui.GetWindowHeight() - 96 ), false ) )
			{
				if ( ImGui.BeginTable( "Output", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable ) )
				{
					ImGui.TableSetupColumn( "Name", ImGuiTableColumnFlags.WidthFixed, 96 );
					ImGui.TableSetupColumn( "Value" );

					ImGui.TableHeadersRow();

					foreach ( var property in item.ClassInfo.Properties.All )
					{
						PropertyGUI( property, item );
					}
				}

				ImGui.EndTable();
			}

			ImGui.EndChild();
		}

		private void PropertyGUI( Property property, ILibrary instance )
		{
			ImGui.TableNextColumn();
			ImGui.Text( property.Name );

			ImGui.TableNextColumn();

			if ( _drawers.ContainsKey( property.Type ) )
			{
				if ( _drawers[property.Type] != null )
				{
					ImGui.BeginGroup();

					ImGui.PushID( property.Name );
					_drawers[property.Type].OnLayout( property, instance );
					ImGui.PopID();

					ImGui.EndGroup();
				}
				else
				{
					ImGui.Text( property[instance]?.ToString() ?? "NULL" );
				}

				return;
			}

			_drawers.Add( property.Type, GrabDrawer( property.Type ) );
		}

		private Drawer GrabDrawer( Type type )
		{
			var lib = Library.Database.Find<Drawer>( e => e.Components.Get<TargetAttribute>()?.Type == type );

			return lib == null ? null : Library.Database.Create<Drawer>( lib.Info );

		}

		//
		// UI
		// 

		private static Dictionary<Type, Drawer> _drawers = new();

		[Singleton, Group( "Inspector" )]
		public abstract class Drawer : ILibrary
		{
			public Library ClassInfo { get; }

			public Drawer()
			{
				ClassInfo = Library.Register( this );
			}

			public abstract void OnLayout( Property property, ILibrary instance );
		}
	}
}
