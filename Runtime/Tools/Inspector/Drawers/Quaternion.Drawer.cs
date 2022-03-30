using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( Quaternion ) )]
	internal class QuaternionDrawer : Inspector.Drawer<Quaternion>
	{

		protected override bool OnLayout( object instance, in Quaternion value, out Quaternion change )
		{
			ImGui.Text( value.ToString() );

			change = Quaternion.identity;
			return false;
		}
	}
}
