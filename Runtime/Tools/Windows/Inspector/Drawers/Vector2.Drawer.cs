using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( Vector2 ) )]
	internal class Vector2Drawer : Inspector.Drawer<Vector2>
	{
		protected override bool OnLayout( object instance, in Vector2 value, out Vector2 change )
		{
			var newValue = value;

			ImGui.InputFloat2( string.Empty, ref newValue );

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
