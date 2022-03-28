using System;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	[Target( typeof( string ) )]
	internal class StringDrawer : Inspector.Drawer
	{
		public override void OnLayout( Property item, ILibrary instance )
		{
			var currentValue = item[instance];

			if ( currentValue == null )
			{
				ImGui.Text( "Null" );
				return;
			}

			var lastValue = (string)currentValue;

			if ( !item.Editable )
			{
				ImGui.TextWrapped( lastValue );
				return;
			}

			var value = lastValue;

			ImGui.InputText( string.Empty, ref value, 160 );

			if ( value != lastValue )
			{
				Dev.Log.Info("Value Changed");
				item[instance] = value;
			}
		}
	}
}
