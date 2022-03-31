using ImGuiNET;

namespace Espionage.Engine.Tools
{
	[Target( typeof( string ) )]
	internal class StringDrawer : Inspector.Drawer<string>
	{
		protected override bool OnLayout( object instance, in string value, out string change )
		{
			if ( value == null )
			{
				change = string.Empty;
				return true;
			}

			var newValue = value;

			if ( Property is { Editable: false } )
			{
				ImGui.TextWrapped( value );

				change = default;
				return false;
			}

			ImGui.InputText( string.Empty, ref newValue, 160 );

			if ( value != newValue )
			{
				change = newValue;
				return true;
			}

			change = default;
			return false;
		}
	}
}
