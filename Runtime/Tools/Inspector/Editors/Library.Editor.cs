using ImGuiNET;

namespace Espionage.Engine.Tools.Editors
{
	[Target( typeof( Library ) )]
	internal class LibraryEditor : Inspector.Editor
	{
		public override void OnLayout( object item )
		{
			var info = item as Library;
			ImGui.Text( info.Name );
		}
	}
}
