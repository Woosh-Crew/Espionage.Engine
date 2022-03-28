using System.Collections;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	[Target( typeof( IDatabase<> ) )]
	public class CollectionDrawer : Inspector.Drawer
	{
		public override void OnLayout( Property property, ILibrary instance )
		{
			ImGui.Text( "Collection" );
		}
	}
}
