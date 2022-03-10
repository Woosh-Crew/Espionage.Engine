using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Espionage.Engine.AI
{
	/// <summary>
	/// An AINodeGraph holds a list of all the AINodes in a given graph and is responsible for returning and calculating path queries
	/// </summary>
    public class AINodeGraph : Component<World>
    {
		/// <summary>
		/// The list of Nodes in our graph
		/// </summary>
		private List<AINode> Nodes{get; set;}

		/// <summary>
		/// This is responsible for genrating the nodegraph for a given type of AINode
		/// </summary>
		public virtual void Generate<AINode>(){

		}

		/// <summary>
		/// Returns the node in the graph nearest to the given point
		/// </summary>
		public virtual AINode NodeFromWorldPoint(Vector3 worldPosition){
			//Return the closest node to the given world position
			return Nodes.OrderBy( x => Vector3.Distance( x.transform.position, worldPosition ) ).FirstOrDefault();
		}
    }
}
