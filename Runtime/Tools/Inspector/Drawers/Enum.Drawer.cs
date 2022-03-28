using System;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	[Target( typeof( Enum ) )]
	internal class EnumDrawer : Inspector.Drawer
	{
		public override void OnLayout( Property item, ILibrary instance )
		{
			if ( ImGui.Selectable( item[instance].ToString() ) ) { }

			if ( ImGui.BeginPopupContextItem( "enum_choice", ImGuiPopupFlags.MouseButtonLeft ) )
			{
				ImGui.Text( item.Type.Name );
				ImGui.Separator();

				foreach ( var name in Enum.GetNames( item.Type ) )
				{
					if ( ImGui.Selectable( name ) )
					{
						item[instance] = Enum.Parse( item.Type, name );
					}
				}

				ImGui.EndPopup();
			}
		}
	}
}
