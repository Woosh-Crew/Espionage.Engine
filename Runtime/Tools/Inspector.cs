using System;
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
			foreach ( var property in item.ClassInfo.Properties.All )
			{
				PropertyGUI( property );
			}
		}

		private void PropertyGUI( Property property )
		{
			ImGui.Text( property.Name );
			ImGui.SameLine();
			ImGui.Value( "", 25 );
		}
	}
}
