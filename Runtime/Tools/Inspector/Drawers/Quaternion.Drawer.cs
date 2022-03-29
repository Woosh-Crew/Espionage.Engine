using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( Quaternion ) )]
	internal class QuaternionDrawer : Inspector.Drawer
	{
		public override void OnLayout( Property item, object instance )
		{
			var currentValue = item[instance];

			if ( currentValue == null )
			{
				ImGui.Text( "Null" );
				return;
			}

			ImGui.Text( currentValue.ToString() );
		}
	}
}
