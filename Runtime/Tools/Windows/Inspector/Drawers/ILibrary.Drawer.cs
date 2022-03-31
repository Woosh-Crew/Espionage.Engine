using Espionage.Engine.Services;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( ILibrary ) )]
	internal class ILibraryDrawer : Inspector.Drawer<ILibrary>
	{
		protected override bool OnLayout( object instance, in ILibrary value, out ILibrary change )
		{
			if ( value == null )
			{
				ImGui.Text( "Null" );

				change = null;
				return false;
			}

			if ( ImGui.Selectable( value.ToString() ) )
			{
				Engine.Services.Get<DiagnosticsService>().Selection = value;
			}

			ImGui.SameLine();
			ImGui.TextColored( Color.gray, $" [{value.ClassInfo.Title}]" );

			change = null;
			return false;
		}
	}
}
