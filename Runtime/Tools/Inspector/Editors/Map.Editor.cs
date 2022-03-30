using Espionage.Engine.Resources;
using ImGuiNET;

namespace Espionage.Engine.Tools.Editors
{
	[Target( typeof( Map ) )]
	internal class MapEditor : Inspector.Editor
	{
		public override void OnLayout( object item )
		{
			if ( item is not Map map )
			{
				ImGui.Text( "Map was NULl" );
				return;
			}

			ImGui.Text( $"Map {map.Identifier}" );
		}
	}
}
