using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Target( typeof( Vector3 ) )]
	internal class Vector3Drawer : Inspector.Drawer<Vector3>
	{
		protected override bool OnLayout( object instance, in Vector3 value, out Vector3 change )
		{
			var newValue = value;

			ImGui.InputFloat3( string.Empty, ref newValue );

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
