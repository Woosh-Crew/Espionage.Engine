using System;
using UnityEditor.Experimental.GraphView;

namespace Espionage.Engine.Nodes.Editor
{
	public class PortUI : Port
	{
		protected PortUI( Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type ) : base( portOrientation, portDirection, portCapacity, type )
		{
		}
	}
}
