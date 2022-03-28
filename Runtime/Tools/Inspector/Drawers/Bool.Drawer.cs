using ImGuiNET;

namespace Espionage.Engine.Tools
{
	[Target( typeof( bool ) )]
	internal class BoolDrawer : Inspector.Drawer
	{
		public override void OnLayout( Property item, ILibrary instance )
		{
			var currentValue = item[instance];

			if ( currentValue == null )
			{
				ImGui.Text("Null");
				return;
			}
			
			var lastValue = (bool)currentValue;
			var value = lastValue;
			
			ImGui.Checkbox( string.Empty, ref value );

			if ( value != lastValue )
			{
				item[instance] = value;
			}
		}
	}
}
