using System.Collections;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	[Target( typeof( IEnumerable ) )]
	public class CollectionDrawer : Inspector.Drawer
	{
		public override void OnLayout( Property property, object instance )
		{
			ImGui.Text( "Collection" );
		}
	}
}
