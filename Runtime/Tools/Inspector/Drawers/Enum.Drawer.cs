using System;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( Enum ) )]
	internal class EnumDrawer : Inspector.Drawer
	{
		public override void OnLayout( Property item, ILibrary instance )
		{
			ImGui.Text( "Enum" );
		}
	}
}
