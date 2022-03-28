using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( Vector2 ) )]
	internal class Vector2Drawer : Inspector.Drawer
	{
		public override void OnLayout( Property item, ILibrary instance )
		{
			var currentValue = item[instance];

			if ( currentValue == null )
			{
				ImGui.Text( "Null" );
				return;
			}
			
			var lastValue = (Vector2)currentValue;
			var value = lastValue;
			
			ImGui.InputFloat2( string.Empty, ref value );

			if ( value != lastValue )
			{
				item[instance] = value;
			}
		}
	}
}
