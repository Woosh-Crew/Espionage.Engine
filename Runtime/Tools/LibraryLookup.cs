using ImGuiNET;

namespace Espionage.Engine.Tools
{
	public class LibraryLookup : Window
	{
		public override void OnLayout()
		{
			ImGui.BeginChild( "Output", new( 0, 0 ), true, ImGuiWindowFlags.ChildWindow );
			{
				foreach ( var library in Library.Database.All )
				{
					if ( ImGui.Selectable( $"{library.Title}" ) )
					{
						Service.Selection = library;
					}
				}
			}
			ImGui.EndChild();
		}
	}
}
