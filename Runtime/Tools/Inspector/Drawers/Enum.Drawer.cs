using System;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	[Target( typeof( Enum ) )]
	internal class EnumDrawer : Inspector.Drawer
	{
		public override bool OnLayout( object instance, in object value, out object change )
		{
			if ( ImGui.Selectable( value.ToString() ) ) { }

			if ( ImGui.BeginPopupContextItem( "enum_choice", ImGuiPopupFlags.MouseButtonLeft ) )
			{
				ImGui.Text( Type.Name );
				ImGui.Separator();

				foreach ( var name in Enum.GetNames( Type ) )
				{
					if ( !ImGui.Selectable( name ) )
					{
						continue;
					}

					change = Enum.Parse( Type, name );
					return true;
				}

				ImGui.EndPopup();
			}

			change = null;
			return false;
		}
	}
}
