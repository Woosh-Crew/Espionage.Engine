using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Espionage.Engine.Entities;

using Graph = UnityEditor.Experimental.GraphView;
using Node = Espionage.Engine.Entities.Node;

namespace Espionage.Engine.Internal.Editor
{
	public class BlueprintNodeUI : Graph.Node
	{
		private Node _node;

		public BlueprintNodeUI( Node node )
		{
			_node = node;
		}
	}
}
